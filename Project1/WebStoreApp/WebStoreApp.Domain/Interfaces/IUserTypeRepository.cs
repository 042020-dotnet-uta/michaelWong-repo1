using System;
using System.Threading.Tasks;

namespace WebStoreApp.Domain.Interfaces
{
    public interface IUserTypeRepository : IRepository<UserType>
    {
        Task<UserType> GetByName (string name);
    }
}