using MediatR;

namespace Sympli.Models
{
    public class SearchRequest : IRequest<string>
    {
        public string Search { get; set; }
        public string SearchUrl { get; set; }
        public int SearchEngineType { get; set; }
    }
}
