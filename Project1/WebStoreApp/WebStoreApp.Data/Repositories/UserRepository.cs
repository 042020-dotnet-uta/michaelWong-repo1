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

        public async Task<User> VerifyLogin(string username, string password)
        {
            var users = await Get(user => user.LoginInfo.Username == username);
            if (users.Any())
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: users.ElementAt(0).LoginInfo.Salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));
                if (users.ElementAt(0).LoginInfo.Hashed == hashed) return users.ElementAt(0);
            }
            return null;
        }
    }
}