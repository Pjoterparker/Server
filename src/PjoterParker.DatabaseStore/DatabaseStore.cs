using System.Linq;
using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Extensions;
using PjoterParker.Database;
using PjoterParker.Domain.Locations;

namespace PjoterParker.DatabaseStore
{
    public class DatabaseStore
    {
        private readonly IApiDatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public DatabaseStore(IApiDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Map(LocationAggregate locationAggregate)
        {
            var location = _dbContext.Location.FirstOrDefault(l => l.LocationId == locationAggregate.Id);
            if (location.IsNull())
            {
                location = _mapper.Map<Location>(locationAggregate);
                _dbContext.Location.Add(location);
            }
            else
            {
                location = _mapper.Map<Location>(locationAggregate);
                _dbContext.Location.Update(location);
            }

            _dbContext.SaveChanges();
        }

        public void Map(SpotAggregate spotAggregate)
        {
            var spot = _dbContext.Spot.FirstOrDefault(l => l.LocationId == spotAggregate.Id);
            if (spot.IsNull())
            {
                spot = _mapper.Map<Spot>(spotAggregate);
                _dbContext.Spot.Add(spot);
            }
            else
            {
                spot = _mapper.Map<Spot>(spotAggregate);
                _dbContext.Spot.Update(spot);
            }

            _dbContext.SaveChanges();
        }
    }
}