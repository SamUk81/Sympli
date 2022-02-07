using Sympli.Models;
using System.Threading.Tasks;

namespace Sympli.Contracts
{
    public interface IGoogleService
    {
        Task<string> Search(SearchRequest request);
    }
}
