using Library.API.Entities;
using Library.API.Helpers;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public interface ICommodityService
    {
        Task<PagedList<Commodity>> GetAllAsync(ResourceParameters parameters);
    }
}
