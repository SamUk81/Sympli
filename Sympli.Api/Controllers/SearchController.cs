using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Sympli.Models;
using Sympli.Contracts.Enums;
using System;

namespace Sympli.Api.Controllers
{
    [EnableCors("CorsApi")]
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(string searchKeyword, string searchUrl, SearchEngineType enumSearchEngineType = SearchEngineType.Google)
        {
            var searchRequest = new SearchRequest()
            {
                Search = searchKeyword,
                SearchUrl = searchUrl,
                SearchEngineType = (int)enumSearchEngineType
            };
            
            var response = await _mediator.Send(searchRequest);

            return response;
        }
    }
}
