using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.Discount
{
    public interface IDiscountClient
    {
        Task<CouponModel> GetDiscount(string productName);
    }
}
