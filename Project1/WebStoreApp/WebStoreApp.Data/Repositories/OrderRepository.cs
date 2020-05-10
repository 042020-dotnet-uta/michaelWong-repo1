using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;

namespace WebStoreApp.Data.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(WebStoreAppContext context)
            : base(context)
        {
            dbSet = (DbSet<Order>)dbSet
                .Include(o => o.Product)
                .ThenInclude(p => p.Location)
                .Include(o => o.User);
        }
    }
}