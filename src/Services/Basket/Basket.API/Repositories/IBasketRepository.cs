using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> Get(string userName);
        Task Update(ShoppingCart cart);
        Task Delete(string userName);
    }
}
