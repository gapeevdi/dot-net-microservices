using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderedRepository
    {
        public OrderRepository(OrderContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orders = await Context.Orders
                .Where(order => order.UserName == userName)
                .ToListAsync();

            return orders;
        }
    }
}
