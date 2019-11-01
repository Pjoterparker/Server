using AutoMapper;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Domain.Locations
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