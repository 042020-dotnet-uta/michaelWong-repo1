using System;
using System.Linq;
using System.Collections.Generic;
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
                .SingleOrDefaultAsync(user => user.Id == (Guid)id);
        }

        public async virtual Task<User> GetByUsername(string username)
        {
            return await dbSet
                .Include(user => user.UserInfo)
                .Include(user => user.UserType)
                .Include(user => user.LoginInfo)
                .SingleOrDefaultAsync(user => user.LoginInfo.Username == username);
        }

        public async virtual Task<List<User>> SearchUsers(string firstName, string lastName)
        {
            return await dbSet
                .Include(user => user.UserType)
                .Include(user => user.UserInfo)
                .Where(user => user.UserType.Name != "Admin")
                .Where(user => user.UserInfo.FirstName.ToUpper().Contains(firstName.ToUpper()))
                .Where(user => user.UserInfo.LastName.ToUpper().Contains(lastName.ToUpper()))
                .ToListAsync();
        }
    }
}