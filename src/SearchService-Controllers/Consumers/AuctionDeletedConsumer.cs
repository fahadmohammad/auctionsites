using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly DB _db;


        public AuctionDeletedConsumer(DB db)
        {
            _db = db;

        }
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine("--> Consuming AuctionDeleted: " + context.Message.Id);

            var result = await _db.DeleteAsync<Item>(context.Message.Id);

            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
        }
    }
}