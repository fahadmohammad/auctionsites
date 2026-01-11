using MongoDB.Entities;
using SearchService_Controllers.Data;
using SearchService_Controllers.Models;

namespace SearchService_Controllers.Services
{
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Item>> GetItemsforSearchDb(string lastUpdated)
        {
            return await _httpClient.GetFromJsonAsync<List<Item>>(_configuration["AuctionServiceUrl"] + $"/api/auctions?date={lastUpdated}");
        }
    }
}