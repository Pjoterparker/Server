using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Extensions;
using PjoterParker.Database;
using PjoterParker.Domain.Locations;

namespace PjoterParker.DatabaseStore
{
    public class DatabaseAggregateStore :
        IAggregateMap<LocationAggregate>,
        IAggregateMap<SpotAggregate>,
        IAggregateDbStore
    {
        private readonly IApiDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IComponentContext _context;

        private readonly Dictionary<string, Action<IComponentContext, IAggregateRoot>> _storeMethods = new Dictionary<string, Action<IComponentContext, IAggregateRoot>>();

        public DatabaseAggregateStore(IApiDatabaseContext dbContext, IMapper mapper, IComponentContext context)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _context = context;

            AddStoreMethod<LocationAggregate>();
            AddStoreMethod<SpotAggregate>();
        }

        public void AddStoreMethod<TAggregate>() where TAggregate : class, IAggregateRoot
        {
            _storeMethods.Add(typeof(TAggregate).Name, (context, aggregate) => { _context.Resolve<IAggregateMap<TAggregate>>().Save(aggregate as TAggregate); });
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

        public void Save(SpotAggregate spotAggregate)
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

        public Task SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregateRoot
        {
            (_storeMethods[aggregate.GetType().Name])(_context, aggregate);
            return Task.CompletedTask;
        }
    }
}