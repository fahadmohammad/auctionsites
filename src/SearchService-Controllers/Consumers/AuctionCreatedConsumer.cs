using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService_Controllers.Data;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        private readonly DB _db;

        public AuctionCreatedConsumer(IMapper mapper, DB db)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            System.Console.WriteLine($"--> Consume AuctionCreated event: {context.Message.Id}");

            var item = _mapper.Map<Item>(context.Message);

            if(item.Model == "foo")
            {
                throw new ArgumentException("Test exception for retry");
            }

            await _db.SaveAsync(item);
        }
    }
}