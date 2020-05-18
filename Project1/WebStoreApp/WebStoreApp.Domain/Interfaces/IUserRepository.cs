using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdFull(object id);
        Task<User> GetByUsername(string username);
        Task<List<User>> SearchUsers(string firstName, string lastName);
    }
}