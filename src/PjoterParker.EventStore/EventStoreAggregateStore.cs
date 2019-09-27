using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;
using PjoterParker.Core.Validation;
using StackExchange.Redis;

namespace PjoterParker.Core.EventStore
{
    public class EventStoreAggregateStore : IAggregateStore
    {
        private const int READ_PAGE_SIZE = 500;

        private const long WRITE_PAGE_SIZE = 500;

        private readonly IDatabase _cache;

        private readonly IComponentContext _context;

        private readonly IEventStoreConnection _eventStore;

        public EventStoreAggregateStore(IEventStoreConnection eventStore, IDatabase cache, IComponentContext context)
        {
            _eventStore = eventStore;
            _cache = cache;
            _context = context;
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid AggregateId) where TAggregate : IAggregateRoot, new()
        {
            return await GetByIdAsync<TAggregate>(AggregateId, int.MaxValue);
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid AggregateId, int version) where TAggregate : IAggregateRoot, new()
        {
            if (version <= 0)
            {
                throw new InvalidOperationException("Cannot get version <= 0");
            }

            var streamName = $"{typeof(TAggregate).Name}:{AggregateId}";
            TAggregate aggregate = new TAggregate();

            var value = await _cache.StringGetAsync(streamName);
            if (value.HasValue)
            {
                aggregate = JsonConvert.DeserializeObject<TAggregate>(value);
            }

            long sliceStart = aggregate.Version + 1;
            StreamEventsSlice currentSlice = null;

            do
            {
                int sliceCount = READ_PAGE_SIZE;
                currentSlice = await _eventStore.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount, false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                {
                    throw new AppValidationException($"{typeof(TAggregate).Name}Id", $"Aggragate with id : {aggregate.Id} not found");
                }

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                {
                    throw new AppValidationException($"{typeof(TAggregate).Name}Id", $"Aggragate with id : {aggregate.Id} deleted");
                }

                sliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                {
                    var realEvent = DeserializeEvent(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data);
                    aggregate.Apply(realEvent as IEvent);
                    aggregate.Version++;
                }
            }
            while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            aggregate.Context = _context;
            return aggregate;
        }

        public TAggregate GetNew<TAggregate>() where TAggregate : IAggregateRoot, new()
        {
            var model = new TAggregate();
            model.Context = _context;
            return model;
        }

        public async Task SaveAsync(IAggregateRoot aggregate)
        {
            var streamName = $"{aggregate.GetType().Name}:{aggregate.Id}";
            var newEvents = aggregate.Events;
            var eventsToSave = newEvents.Select(e => ToEventData(e)).ToList();

            await _eventStore.AppendToStreamAsync(streamName, aggregate.Version, eventsToSave);

            aggregate.Version += aggregate.Events.Count();
            await _cache.StringSetAsync(streamName, JsonConvert.SerializeObject(aggregate), TimeSpan.FromDays(1));
        }

        private object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventType = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property("EventType").Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventType));
        }

        private EventData ToEventData(EventComposite @event)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event.Event));
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event.Metadata));
            var typeName = @event.GetType().Name;

            return new EventData(@event.Metadata.EventId, typeName, true, data, metadata);
        }
    }
}
