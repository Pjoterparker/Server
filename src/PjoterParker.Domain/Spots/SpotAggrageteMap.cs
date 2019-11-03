using AutoMapper;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Domain.Spots
{
    public class SpotAggregateToSpot : Profile
    {
        public SpotAggregateToSpot()
        {
            CreateMap<SpotAggregate, Spot>()
               .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => src.Id));

            //CreateMap<ReservationEntity, Reservation>();
        }
    }
}