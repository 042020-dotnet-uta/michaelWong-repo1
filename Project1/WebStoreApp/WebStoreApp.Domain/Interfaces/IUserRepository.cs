using System;
using System.Threading.Tasks;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> VerifyLogin(string username, string password);
    }
}