using System.Threading.Tasks;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> Get(string productName);
        Task Create(Coupon coupon);
        Task Update(Coupon coupon);
        Task Delete(string productName);
    }
}
