using Sympli.Models;
using System.Threading.Tasks;

namespace Sympli.Contracts
{
    public interface IBingService
    {
        Task<string> Search(SearchRequest request);
    }
}
