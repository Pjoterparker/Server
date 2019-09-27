using System.Linq;
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
        IEventHandlerAsync<OwnerAssignedToSpot>,
        IEventHandlerAsync<MarkSpotAsUnoccupied>,
        IEventHandlerAsync<PropertyChanged<SpotAggregate>>
    {
        private readonly IAggregateStore _aggregateStore;

        private readonly IApiDatabaseContext _database;

        private readonly IMapper _mapper;

        public SpotHandler(IApiDatabaseContext database)
        {
            _database = database;
        }

        public Task HandleAsync(SpotCreated @event)
        {
            _database.Spot.Add(new Spot(@event));
            _database.SaveChanges();

            return Task.CompletedTask;
        }

        public Task HandleAsync(MarkSpotAsUnoccupied @event)
        {
            var spot = _database.Spot.First(l => l.SpotId == @event.SpotId);
            spot.UserId = null;

            _database.Spot.Update(spot);
            _database.SaveChanges();

            return Task.CompletedTask;
        }

        public Task HandleAsync(OwnerAssignedToSpot @event)
        {
            var spot = _database.Spot.First(l => l.SpotId == @event.SpotId);
            spot.UserId = @event.OwnerId;

            _database.Spot.Update(spot);
            _database.SaveChanges();

            return Task.CompletedTask;
        }

        public Task HandleAsync(PropertyChanged<SpotAggregate> @event)
        {
            var spot = _database.Spot.First(l => l.SpotId == @event.AggregateId);
            typeof(Spot).GetProperty(@event.PropertyName).SetValue(spot, @event.NewValue);
            _database.Spot.Update(spot);
            _database.SaveChanges();

            return Task.CompletedTask;
        }
    }
}