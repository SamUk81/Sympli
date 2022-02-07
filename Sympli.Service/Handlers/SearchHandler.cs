using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System;
using Sympli.Models;
using Sympli.Contracts;
using Sympli.Contracts.Enums;

namespace Sympli.Services.Handlers
{
    public class SearchHandler : IRequestHandler<SearchRequest, string>
    {
        private readonly IGoogleService _googleService;
        private readonly IBingService _bingService;

        public SearchHandler(IGoogleService googleService, IBingService bingService)
        {
            _googleService = googleService;
            _bingService = bingService;
        }

        public async Task<string> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SearchEngineType == (int)SearchEngineType.Google)
                {
                    var GoogleSearchResultUrls = await _googleService.Search(request);
                    return GoogleSearchResultUrls;
                }
                else
                {
                    var BingSearchResultUrls = await _bingService.Search(request);
                    return BingSearchResultUrls;
                }
            }

            catch (Exception ex)
            {
                // Logger.log(ex.Message);
            }

            return "exception occured, we are looking into it";
        }

    }
}
