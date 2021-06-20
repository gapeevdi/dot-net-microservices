using System;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.Discount
{
    public class DiscountClient : IDiscountClient
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountClient(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ??
                                          throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }


        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest{ProductName = productName};
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }



    }
}
