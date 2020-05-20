using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Xunit;
using WebStoreApp.Data;
using WebStoreApp.Domain;

namespace WebStoreApp.Testing
{
    public partial class UnitOfWorkTests
    {
        [Fact]
        public async Task AddUserTest()
        {
            // Arrange
            var options = SetUp("AddUserTest");

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.UserRepository.Insert(new User
                {
                    UserType = new UserType()
                });
                await unitOfWork.Save();
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                Assert.Single(await unitOfWork.UserRepository.All());
            }
        }

        [Fact]
        public async Task SearchUsersTest()
        {
            // Arrange
            var options = SetUp("SearchUsersTest");
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.UserRepository.Insert(new User
                {
                    UserType = new UserType(),
                    UserInfo = new UserInfo
                    {
                        FirstName = "John",
                        LastName = "Smith"
                    },
                    LoginInfo = new LoginInfo
                    {
                        Username = "Username"
                    }
                });
                await unitOfWork.Save();
            }
            User usernameTest; User idTest; User searchTest;

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                usernameTest = await unitOfWork.UserRepository.GetByUsername("Username");
                idTest = await unitOfWork.UserRepository.GetById(usernameTest.Id);
                searchTest = (await unitOfWork.UserRepository.SearchUsers("John", "Smith"))[0];
            }

            // Assert
            {
                Assert.NotNull(usernameTest);
                Assert.NotNull(idTest);
                Assert.NotNull(searchTest);
            }
        }

        [Fact]
        public async Task UpdateTest()
        {
            // Arrange
            var options = SetUp("UpdateTest");
            Guid id;
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                id = (await unitOfWork.UserRepository.Insert(new User
                {
                    UserInfo = new UserInfo(),
                    UserType = new UserType()
                })).Id;
                await unitOfWork.Save();
            }

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                var user = await unitOfWork.UserRepository.GetByIdFull(id);
                user.UserInfo.FirstName = "Bob";
                await unitOfWork.UserRepository.Update(user);
                await unitOfWork.Save();
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                var user = await unitOfWork.UserRepository.GetByIdFull(id);
                Assert.Equal("Bob", user.UserInfo.FirstName);
            }
        }
    }
}