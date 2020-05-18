using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebStoreApp.Web.Models;
using WebStoreApp.Domain;
using WebStoreApp.Data;

namespace WebStoreApp.Web.Services
{
    public class UserModelService : IUserModelService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserModelService(WebStoreAppContext context)
        {
            this._unitOfWork = new UnitOfWork(context);
        }

        public async Task<User> VerifyLogin(LoginModel loginModel)
        {
            var user = await _unitOfWork.UserRepository.GetByUsername(loginModel.UsernameLogin);
            if (user == null) throw new KeyNotFoundException("A user with matching username and password was not found.");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginModel.PasswordLogin,
                salt: user.LoginInfo.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 50000,
                numBytesRequested: 256 / 8
            ));
            if (user.LoginInfo.Hashed == hashed) return user;
            throw new KeyNotFoundException("A user with matching username and password was not found.");
        }

        public async Task RegisterUser(RegisterModel registerModel)
        {
            if (registerModel.Password != registerModel.PasswordConfirmation) throw new InvalidOperationException("Those passwords do not match.");
            if (await _unitOfWork.UserRepository.GetByUsername(registerModel.Username) != null) throw new InvalidOperationException("A user with that username already exists.");
            var userInfo = new UserInfo
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            var userType = await _unitOfWork.UserTypeRepository.GetByName("Customer");
            if (userType == null) throw new KeyNotFoundException("User type was not found. Unable to create new customers at this time.");

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: registerModel.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 50000,
                numBytesRequested: 256 / 8
            ));

            var loginInfo = new LoginInfo
            {
                Username = registerModel.Username,
                Salt = salt,
                Hashed = hashed
            };

            var user = new User
            {
                UserInfo = userInfo,
                UserType = userType,
                LoginInfo = loginInfo
            };

            user = (await _unitOfWork.UserRepository.Insert(user));
            await _unitOfWork.Save();
        }

        public async Task<UserModel> GetUserDetails(Guid? id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdFull(id);
            if (user == null) throw new KeyNotFoundException("User was not found.");
            var userModel = new UserModel
            {
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Username = user.LoginInfo.Username
            };
            return userModel;
        }

        public async Task<OrdersModel> GetUserOrders(Guid? id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null) throw new KeyNotFoundException("User was not found.");

            var orders = await _unitOfWork.OrderRepository.GetByUser(id);
            var ordersModel = new OrdersModel
            {
                OrderModels = new List<OrderModel>()
            };
            foreach (var order in orders)
            {
                var orderModel = new OrderModel
                {
                    LocationName = order.Location.Name,
                    ProductName = order.OrderInfo.ProductName,
                    ProductPrice = order.OrderInfo.ProductPrice,
                    Quantity = order.OrderInfo.ProductQuantity,
                    Timestamp = order.Timestamp
                };
                ordersModel.OrderModels.Add(orderModel);
            }

            return ordersModel;
        }

        public async Task<List<User>> SearchUsers(string firstName, string lastName)
        {
            return await _unitOfWork.UserRepository.SearchUsers(firstName, lastName);
        }
    }
}