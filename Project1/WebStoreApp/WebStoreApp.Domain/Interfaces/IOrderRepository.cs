using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByLocation(object id);
        Task<IEnumerable<Order>> GetByUser(object id);
    }
}