using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;

namespace WebStoreApp.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(WebStoreAppContext context)
            : base(context)
        {
            dbSet = (DbSet<Product>)dbSet
                .Include(p => p.Location);
        }
    }
}