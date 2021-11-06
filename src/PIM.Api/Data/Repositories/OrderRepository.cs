using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PIM.Api.Core.Models;
using PIM.Api.Data.Context;
using PIM.Api.Data.Repositories.Base;

namespace PIM.Api.Data.Repositories
{
    public class OrderRepository: BaseRepository<Order>
    {
        public OrderRepository(ApplicationContext db) : base(db) { }

        public IEnumerable<Order> GetOrders()
        {
            return Db.Orders.Include(o=> o.Products).ThenInclude(p=> p.Product);
        }
    }
}
