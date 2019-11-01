using System.Linq;
using AutoMapper;
using PjoterParker.Api.Database;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Common.Extensions;
using PjoterParker.Core.Aggregates;

namespace PjoterParker.Domain.Locations
{
    public class LocationToDb : IAggregateMap<LocationAggregate>
    {
        private readonly IApiDatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public LocationToDb(IApiDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Save(LocationAggregate locationAggregate)
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
    }
}