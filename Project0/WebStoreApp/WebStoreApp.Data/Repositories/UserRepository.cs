using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;

namespace WebStoreApp.Data
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(WebStoreAppContext context)
            : base(context)
        {
            dbSet = (DbSet<User>)dbSet
                .Include(u => u.UserType)
                .Include(u => u.UserInfo)
                .Include(u => u.LoginInfo);
        }

        public async override Task<User> Insert(User user)
        {
            if (_context.Entry(user.UserType).State == EntityState.Detached)
            {
                _context.UserTypes.Attach(user.UserType);
            }
            var entity = dbSet.Add(user).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}