using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using Sympli.Models;
using Sympli.Contracts;

namespace Sympli.Services
{
    public class GoogleService : IGoogleService
    {
        private static CacheService<string> _searchCacheService = new CacheService<string>();
        private readonly IHttpClientFactory _clientFactory;

        public GoogleService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> Search(SearchRequest request)
        {
            var searchCache =
            await _searchCacheService.GetOrCreate($"{request.Search}:{request.SearchUrl}", async () => await GoogleSearchResultUrls(request));

            return searchCache;
        }

        private async Task<string> GoogleSearchResultUrls(SearchRequest request)
        {
            const string SearchUrl = "https://www.google.com/";
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(SearchUrl);

            var response = await client.GetStringAsync($"search?num=100&q={request.Search}");

            string regularExpression = @"(div class=\""egMi0 kCrYT\""><a href=\""\/url\?q=)(https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b)";
            Regex expression = new Regex(regularExpression);

            var results = expression.Matches(response);

            var resultUrls = results.Select((x, i) => new
            {
                Url = x.Groups[2].Value,
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
