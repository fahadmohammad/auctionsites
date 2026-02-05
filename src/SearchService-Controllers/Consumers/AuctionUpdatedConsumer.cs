using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;
        private readonly DB _db;

        public AuctionUpdatedConsumer(IMapper mapper, DB db)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            System.Console.WriteLine($"--> Consume AuctionUpdated event: {context.Message.Id}");

            var item = _mapper.Map<Item>(context.Message);

            var result = await _db.Update<Item>()
                .Match(a => a.ID == context.Message.Id)
                 .ModifyOnly(x => new
                 {
                     x.Color,
                     x.Make,
                     x.Model,
                     x.Year,
                     x.Mileage
                 }, item)
                .ExecuteAsync();

            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongoDb");
        }
    }
}