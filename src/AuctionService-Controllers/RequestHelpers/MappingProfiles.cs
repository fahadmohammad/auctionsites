using AuctionService_Controllers.Dtos;
using AuctionService_Controllers.Entities;
using AutoMapper;
using Contracts;

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
            CreateMap<AuctionDto, AuctionCreated>();
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionUpdated>();
        }
    }
}