using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly DB _db;

        public AuctionFinishedConsumer(DB db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("--> Consuming bid placed");

            var auction = await _db.Find<Item>().OneAsync(context.Message.AuctionId)
                ?? throw new MessageException(typeof(AuctionFinished), "Cannot retrieve this auction");

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message?.Winner;
                auction.SoldAmount = (int)context.Message?.Amount;
            }

            auction.Status = "Finished";

            await _db.SaveAsync(auction);
        }
    }
}