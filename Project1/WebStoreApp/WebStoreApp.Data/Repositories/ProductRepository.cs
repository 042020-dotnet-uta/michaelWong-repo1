using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;
using WebStoreApp.Domain.Interfaces;

namespace WebStoreApp.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(WebStoreAppContext context)
            : base(context)
        {
            dbSet = (DbSet<Product>)dbSet
                .Include(p => p.Location);
        }

        public async Task<IEnumerable<Product>> GetByLocation(object id)
        {
            return await Get(product => product.Location.Id == (Guid) id);
        }
    }
}