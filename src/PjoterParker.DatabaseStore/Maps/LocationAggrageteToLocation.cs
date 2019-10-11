using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Domain.Locations;

namespace PjoterParker.DatabaseStore.Maps
{
    public class LocationAggrageteToLocation : Profile
    {
        public LocationAggrageteToLocation()
        {
            CreateMap<LocationAggregate, Location>()
               .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => src.Id));
        }
    }
}
