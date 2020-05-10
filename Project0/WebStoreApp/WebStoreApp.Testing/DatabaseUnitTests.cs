using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Xunit;
using WebStoreApp.Data;
using WebStoreApp.Domain;

namespace WebStoreApp.Testing
{
    public class DatabaseUnitTests
    {
        [Fact]
        public void AddingWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "CreateDatabase")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                // Adds Admin type to Database.
                var adminType = context.UserTypes.Add(new UserType { Name = "Admin", Description = "" }).Entity;
                // Add Customer type to Database.
                var customerType = context.UserTypes.Add(new UserType { Name = "Customer", Description = "" }).Entity;
                context.SaveChanges();

                // Add new Admin user to Database.
                string password = "somethingsomething1";
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));
                User user = new User();
                user.UserInfo = new UserInfo { FirstName = "FirstName", LastName = "LastName" };
                user.UserType = adminType;
                user.LoginInfo = new LoginInfo { Username = "somethingName1", Hashed = hashed, Salt = salt };
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Equal(2, context.UserTypes.ToList().Count);
                Assert.Single(context.Users.ToList());
                Assert.Single(context.UserInfos.ToList());
                Assert.Single(context.LoginInfos.ToList());
            }
        }
    }
}
