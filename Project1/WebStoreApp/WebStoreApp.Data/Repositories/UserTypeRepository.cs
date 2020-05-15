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
    public class UserTypeRepository : BaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(WebStoreAppContext context)
            : base(context)
        {

        }

        public async Task<UserType> GetByName(string name)
        {
            return await _context.UserTypes.FirstOrDefaultAsync(userType => userType.Name == name);
        }
    }
}