using AutoMapper;
using Contracts;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AuctionCreated, Item>();
            CreateMap<AuctionUpdated, Item>();
        }
    }
}