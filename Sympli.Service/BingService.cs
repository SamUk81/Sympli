using Sympli.Contracts;
using Sympli.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sympli.Services
{    
    public class BingService : IBingService
    {
        private static CacheService<string> _searchCacheService = new CacheService<string>();
        private readonly IHttpClientFactory _clientFactory;
        private const int SearchResultsPerPage = 10;

        public BingService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> Search(SearchRequest request)
        {
            var searchCache =
            await _searchCacheService.GetOrCreate($"{request.Search}:{request.SearchUrl}", async () => await BingSearchResultUrls(request));

            return searchCache;
        }

        private async Task<string> BingSearchResultUrls(SearchRequest request)
        {
            const string SearchUrl = "https://www.bing.com/";
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(SearchUrl);            

            var response = await client.GetStringAsync($"search?q={request.Search}");

            string regularExpression = "<li\\sclass=\\\"b_algo\\\"[^>]*>(.*?)</li>";            
            Regex expression = new Regex(regularExpression);

            var results = expression.Matches(response);

            var resultUrls = results.Select((x, i) => new
            {
                Url = x.Groups[1].Value,
                Position = i + 1
            })
            ?.Where(x => x.Url.Contains(request.SearchUrl, StringComparison.OrdinalIgnoreCase))
            ?.ToArray()
            ?.Select(x => x.Position)
            ?.ToArray();

            return resultUrls != null && resultUrls.Any() ? string.Join(",", resultUrls) : "not found";
        }
    }
}
