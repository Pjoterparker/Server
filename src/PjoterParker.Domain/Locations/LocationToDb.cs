using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Extensions;
using PjoterParker.Database;

namespace PjoterParker.Domain.Locations
{
    public class LocationToTable
    {
        private readonly IApiDatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public LocationToTable(IApiDatabaseContext dbContext, IMapper mapper)
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
