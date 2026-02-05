using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService_Controllers.Models;
using SearchService_Controllers.Services;

namespace SearchService_Controllers.Data
{
    public class DbInitializers
    {
        public static async Task SeedDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DB>();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

            // Create indexes
            await db.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await db.CountAsync<Item>();

            var lastUpdated = await db.Find<Item, string>()
                .Sort(x => x.Descending(i => i.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            var items = await httpClient.GetItemsforSearchDb(lastUpdated);

            System.Console.WriteLine($"{items.Count} items fetched from Auction Service.");

            if (items.Count > 0)
            {
                await db.SaveAsync(items);
            }
        }
    }
}
