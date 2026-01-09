using AuctionService_Controllers.Dtos;
using AuctionService_Controllers.Entities;
using AutoMapper;

namespace AuctionService_Controllers.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateActionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));
            CreateMap<CreateActionDto, Item>();
        }
    }
}