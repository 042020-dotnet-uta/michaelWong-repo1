using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebStoreApp.Domain;
using WebStoreApp.Web.Models;

namespace WebStoreApp.Web.Services
{
    public interface IUserModelService
    {
        Task<User> VerifyLogin(LoginModel loginModel);
        Task RegisterUser(RegisterModel registerModel);
        Task<UserModel> GetUserDetails(Guid? id);
        Task<OrdersModel> GetUserOrders(Guid? id);
        Task<List<User>> SearchUsers(string FirstName, string LastName);
    }
}