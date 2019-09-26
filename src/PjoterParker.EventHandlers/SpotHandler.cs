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
    public class SpotHandler :
        IEventHandlerAsync<SpotCreated>,
        IEventHandlerAsync<SpotUpdated>
    {
        private readonly IAggregateStore _aggregateStore;

        private readonly IApiDatabaseContext _database;

        private readonly IMapper _mapper;

        public SpotHandler(IApiDatabaseContext database, IAggregateStore repository, IMapper mapper)
        {
            _database = database;
            _aggregateStore = repository;
            _mapper = mapper;
        }

        public Task HandleAsync(SpotCreated @event)
        {
            _database.Spot.Add(new Spot(@event));
            _database.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task HandleAsync(SpotUpdated @event)
        {
            var spot = await _aggregateStore.GetByIdAsync<SpotAggregate>(@event.SpotId);
            _database.Spot.Update(_mapper.Map<Spot>(spot));
            _database.SaveChanges();
        }
    }
}
