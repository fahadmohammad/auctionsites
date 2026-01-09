using AuctionService_Controllers.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService_Controllers.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Auction> Auctions { get; set; } 
    }
}