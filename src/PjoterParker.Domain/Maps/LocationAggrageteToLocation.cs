using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Domain.Maps
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
