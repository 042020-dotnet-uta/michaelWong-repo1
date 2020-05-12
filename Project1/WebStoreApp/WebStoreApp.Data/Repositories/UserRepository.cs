using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebStoreApp.Domain;
using WebStoreApp.Domain.Interfaces;

namespace WebStoreApp.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(WebStoreAppContext context)
            : base(context)
        {

        }

        public async override Task<User> Insert(User user)
        {
            if (_context.Entry(user.UserType).State == EntityState.Detached)
            {
                _context.UserTypes.Attach(user.UserType);
            }
            var entity = dbSet.Add(user).Entity;
            return await Task.FromResult(entity);
        }

        public async virtual Task<User> GetByIdFull(object id)
        {
            return await dbSet
                .Include(user => user.UserInfo)
                .Include(user => user.LoginInfo)
                .Include(user => user.UserType)
                .SingleOrDefaultAsync(user => user.Id == (Guid) id);
        }
    }
}