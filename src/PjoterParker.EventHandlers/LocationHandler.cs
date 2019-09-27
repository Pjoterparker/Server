using System.Linq;
using System.Threading.Tasks;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Events;
using PjoterParker.Database;
using PjoterParker.Domain.Locations;
using PjoterParker.Events;

namespace PjoterParker.EventHandlers
{
    public class LocationHandler :
        IEventHandlerAsync<LocationCreated>,
        IEventHandlerAsync<PropertyChanged<LocationAggregate>>
    {
        private readonly IApiDatabaseContext _database;

        public LocationHandler(IApiDatabaseContext database)
        {
            _database = database;
        }

        public Task HandleAsync(LocationCreated @event)
        {
            _database.Location.Add(new Location(@event));
            _database.SaveChanges();
            return Task.CompletedTask;
        }

        public Task HandleAsync(PropertyChanged<LocationAggregate> @event)
        {
            var location = _database.Location.First(l => l.LocationId == @event.AggregateId);
            typeof(Location).GetProperty(@event.PropertyName).SetValue(location, @event.NewValue);
            _database.Location.Update(location);
            _database.SaveChanges();

            return Task.CompletedTask;
        }
    }
}