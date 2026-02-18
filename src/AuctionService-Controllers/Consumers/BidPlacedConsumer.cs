using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService_Controllers.Data;
using Contracts;
using MassTransit;

namespace AuctionService_Controllers.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _dbContext;

        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine($"Received BidPlaced event for Auction ID: {context.Message.AuctionId}, Amount: {context.Message.Amount}, Status: {context.Message.BidStatus}");

            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if (auction != null)
            {
                if (auction.CurrentHighBid == null ||
                   context.Message.BidStatus.Contains("Accepted")
                   && context.Message.Amount > auction.CurrentHighBid)
                {
                    auction.CurrentHighBid = context.Message.Amount;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}