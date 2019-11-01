using System.Linq;
using AutoMapper;
using PjoterParker.Api.Database;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Common.Extensions;
using PjoterParker.Core.Aggregates;

namespace PjoterParker.Domain.Spots
{
    public class LocationToDb : IAggregateMap<SpotAggregate>
    {
        private readonly IApiDatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public LocationToDb(IApiDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Save(SpotAggregate spotAggregate)
        {
            var spot = _dbContext.Spot.FirstOrDefault(s => s.SpotId == spotAggregate.Id);
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