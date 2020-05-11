using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByLocation(object id);
    }
}