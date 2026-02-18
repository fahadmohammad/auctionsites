using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService_Controllers.Data;
using AuctionService_Controllers.Entities;
using Contracts;
using MassTransit;

namespace AuctionService_Controllers.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _dbContext;

        public AuctionFinishedConsumer(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            System.Console.WriteLine($"Received AuctionFinished event for Auction ID: {context.Message.AuctionId}, Item Sold: {context.Message.ItemSold}, Amount: {context.Message.Amount}, Winner: {context.Message.Winner}");

            var auctionId = context.Message.AuctionId;
            var auction = await _dbContext.Auctions.FindAsync(auctionId);

            if (auction != null)
            {
                if (context.Message.ItemSold)
                {
                    auction.Winner = context.Message.Winner;
                    auction.SoldAmount = context.Message.Amount;
                }

                auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

                await _dbContext.SaveChangesAsync();

                Console.WriteLine($"Auction with ID {auctionId} marked as Finished.");
            }
            else
            {
                System.Console.WriteLine($"Auction with ID {auctionId} not found.");
            }
        }
    }
}