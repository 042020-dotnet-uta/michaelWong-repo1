using System;
using System.Threading.Tasks;
using System.Linq;
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
            if (user == null) return null;
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginModel.PasswordLogin,
                salt: user.LoginInfo.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 50000,
                numBytesRequested: 256 / 8
            ));
            if (user.LoginInfo.Hashed == hashed) return user;
            return null;
        }

        public async Task<string> RegisterUser(RegisterModel registerModel)
        {
            if (registerModel.Password != registerModel.PasswordConfirmation) return "Passwords don't match.";
            if (await _unitOfWork.UserRepository.GetByUsername(registerModel.Username) != null) return "That username already exists.";
            var userInfo = new UserInfo
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            //TODO USER TYPE
            var userType = new UserType
            {
                Name = "Customer",
                Description = "Something Here"
            };

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
            return null;
        }
    }
}