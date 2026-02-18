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
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly DB _db;

        public BidPlacedConsumer(DB db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("--> Consuming bid placed");

            var auction = await _db.Find<Item>().OneAsync(context.Message.AuctionId)
                ?? throw new MessageException(typeof(AuctionFinished), "Cannot retrieve this auction");

            if ( context.Message.BidStatus.Contains("Accepted")
                && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _db.SaveAsync(auction);
            }
        }
    }
}