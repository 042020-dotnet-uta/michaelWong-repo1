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

        /// <summary>Checks username and password for login</summary>
        /// <param name="loginModel">Login Model containing username and password.</param>
        /// <returns>User instance if login successful.</returns>
        public async Task<User> VerifyLogin(LoginModel loginModel)
        {
            //Checks username.
            var user = await _unitOfWork.UserRepository.GetByUsername(loginModel.UsernameLogin);
            if (user == null) throw new KeyNotFoundException("A user with matching username and password was not found.");

            //Hashes password.
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginModel.PasswordLogin,
                salt: user.LoginInfo.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 50000,
                numBytesRequested: 256 / 8
            ));

            //Verifies password.
            if (user.LoginInfo.Hashed == hashed) return user;
            throw new KeyNotFoundException("A user with matching username and password was not found.");
        }

        /// <summary>Registers new customer.</summary>
        /// <param name="registerModel">Register Model containing new user information.</param>
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

            // Generate salt.
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

            // Hash password.
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

        /// <summary>Gets user details.</summary>
        /// <param name="id">User id.</param>
        /// <returns>User Model that contains user details.</returns>
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

        /// <summary>Get user order history.</summary>
        /// <param name="id">User id.</param>
        /// <returns>Orders Model that contains user order history.</returns>
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

        /// <summary>Searches all users in database by first name and last name.</summary>
        /// <param name="firstName">First name to query.</param>
        /// <param name="lastName">Last name to query.</param>
        /// <returns>List of users.</returns>
        public async Task<List<User>> SearchUsers(string firstName, string lastName)
        {
            return await _unitOfWork.UserRepository.SearchUsers(firstName, lastName);
        }
    }
}