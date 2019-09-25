using System.Threading.Tasks;
using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Database;
using PjoterParker.Domain.Locations;
using PjoterParker.Events;

namespace PjoterParker.EventHandlers
{
    public class LocationHandler :
        IEventHandlerAsync<LocationCreated>,
        IEventHandlerAsync<LocationUpdated>
    {
        private readonly IApiDatabaseContext _database;

        private readonly IAggregateStore _aggregateStore;

        private readonly IMapper _mapper;

        public LocationHandler(IApiDatabaseContext database, IAggregateStore repository, IMapper mapper)
        {
            _database = database;
            _aggregateStore = repository;
            _mapper = mapper;
        }

        public Task HandleAsync(LocationCreated @event)
        {
            _database.Location.Add(new Location(@event));
            _database.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task HandleAsync(LocationUpdated @event)
        {
            var location = await _aggregateStore.GetByIdAsync<LocationAggregate>(@event.LocationId);
            _database.Location.Update(_mapper.Map<Location>(location));
            _database.SaveChanges();
        }
    }
}