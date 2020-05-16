using System;
using System.Threading.Tasks;
using WebStoreApp.Domain;
using WebStoreApp.Web.Models;

namespace WebStoreApp.Web.Services
{
    public interface IUserModelService
    {
        Task<User> VerifyLogin(LoginModel loginModel);
        Task<string> RegisterUser(RegisterModel registerModel);
        Task<UserModel> GetUserDetails(Guid? id);
        Task<OrdersModel> GetUserOrders(Guid? id);
    }
}