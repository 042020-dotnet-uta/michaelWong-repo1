using System;
using System.Threading.Tasks;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdFull(object id);
        Task<User> GetByUsername(string username);
    }
}