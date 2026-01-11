using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService_Controllers.Data;
using SearchService_Controllers.Models;
using SearchService_Controllers.RequestHelpers;

namespace SearchService_Controllers.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly DB _db;
        public SearchController()
        {
            var builder = WebApplication.CreateBuilder();
            _db = DbInitializers.InitDb(builder.Build()).Result;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] RequestParams searchParams)
        {
            var searchTerm = searchParams.SearchTerm;
            var pageSize = searchParams.PageSize;
            var pageNumber = searchParams.PageNumber;
            {
                var query = _db.PagedSearch<Item, Item>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    System.Console.WriteLine($"Searching for items matching: {searchTerm}");
                    query.Match(Search.Full, searchTerm).SortByTextScore();
                }

                query = searchParams.OrderBy switch
                {
                    "make" => query.Sort(x => x.Ascending(i => i.Make)),
                    "new" => query.Sort(x => x.Descending(i => i.CreatedAt)),
                    _ => query.Sort(x => x.Ascending(i => i.AuctionEnd))
                };

                if (!string.IsNullOrEmpty(searchParams.FilterBy))
                {
                    query = searchParams.FilterBy switch
                    {
                        "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                        "endingSoon" => query.Match(x =>
                            x.AuctionEnd < DateTime.UtcNow.AddHours(6)
                                && x.AuctionEnd > DateTime.UtcNow),
                        _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow) // live
                    };
                }

                if (!string.IsNullOrEmpty(searchParams.Seller))
                {
                    query.Match(x => x.Seller == searchParams.Seller);
                }

                if (!string.IsNullOrEmpty(searchParams.Winner))
                {
                    query.Match(x => x.Winner == searchParams.Winner);
                }

                query.PageNumber(pageNumber);
                query.PageSize(pageSize);

                var result = await query.ExecuteAsync();

                return Ok(new
                {
                    results = result.Results,
                    pageCount = result.PageCount,
                    totalCount = result.TotalCount,
                });
            }
        }
    }
}