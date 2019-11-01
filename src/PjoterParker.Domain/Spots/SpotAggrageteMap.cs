using AutoMapper;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Domain.Spots
{
    public class SpotAggrageteToSpot : Profile
    {
        public SpotAggrageteToSpot()
        {
            CreateMap<SpotAggregate, Spot>()
               .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => src.Id));

            CreateMap<ReservationEntity, Reservation>();
        }
    }
}
