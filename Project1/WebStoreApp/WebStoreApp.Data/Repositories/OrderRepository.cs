using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;
using WebStoreApp.Domain.Interfaces;

namespace WebStoreApp.Data.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(WebStoreAppContext context)
            : base(context)
        {
            dbSet = (DbSet<Order>)dbSet
                .Include(o => o.Product)
                .ThenInclude(p => p.Location)
                .Include(o => o.User);
        }

        public async Task<IEnumerable<Order>> GetByLocation(object id)
        {
            return await Get(order => order.Product.Location.Id == (Guid) id);
        }

        public async Task<IEnumerable<Order>> GetByUser(object id)
        {
            return await Get(order => order.User.Id == (Guid) id);
        }
    }
}