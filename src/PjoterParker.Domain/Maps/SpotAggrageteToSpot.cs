using AutoMapper;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Domain.Maps
{
    public class SpotAggrageteToSpot : Profile
    {
        public SpotAggrageteToSpot()
        {
            CreateMap<SpotAggregate, Spot>()
               .ForMember(dest => dest.SpotId, opts => opts.MapFrom(src => src.Id));
        }
    }
}
