using System;
using System.Linq;
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

        }

        public async Task<IEnumerable<Order>> GetByLocation(object id)
        {
            return await dbSet
                .Include(order => order.OrderInfo)
                .Where(order => order.LocationId == (Guid) id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByUser(object id)
        {
            return await dbSet
                .Include(order => order.OrderInfo)
                .Include(order => order.Location)
                .Where(order => order.UserId == (Guid) id)
                .ToListAsync();
        }
    }
}