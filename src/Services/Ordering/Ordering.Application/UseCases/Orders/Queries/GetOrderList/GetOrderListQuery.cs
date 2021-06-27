using System;
using System.Collections.Generic;
using MediatR;

namespace Ordering.Application.UseCases.Orders.Queries.GetOrderList
{
    public class GetOrderListQuery : IRequest<List<OrderResponse>>
    {
        public string UserName { get; private set; }

        public GetOrderListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
