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
    public class DatabaseContextAdditionTests
    {
        [Fact]
        public void AddingUserTypeWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "AddingUserTypeWithDatabaseContext")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                context.UserTypes.Add(new UserType { Name = "Admin", Description = "" });
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                var userTypes = context.UserTypes.ToList();
                Assert.Single(userTypes);
            }
        }

        [Fact]
        public void AddingUserWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "AddingUserWithDatabaseContext")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
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
                user.UserType = new UserType { Name = "", Description = "" };
                user.LoginInfo = new LoginInfo { Username = "somethingName1", Hashed = hashed, Salt = salt };
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Single(context.Users.ToList());
                Assert.Single(context.UserInfos.ToList());
                Assert.Single(context.LoginInfos.ToList());
                Assert.Single(context.UserTypes.ToList());
            }
        }

        [Fact]
        public void AddingLocationWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "AddingLocationWithDatabaseContext")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                context.Locations.Add(new Location { Name = "" });
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                var locations = context.Locations.ToList();
                Assert.Single(locations);
            }
        }

        [Fact]
        public void AddingProductWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "AddingProductWithDatabaseContext")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                var product = new Product { Name = "", Price = 1.00M, Quantity = 1 };
                product.Location = new Location();
                context.Products.Add(product);
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                var products = context.Products.ToList();
                Assert.Single(products);
            }
        }

        [Fact]
        public void AddingOrderWithDatabaseContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "AddingOrderWithDatabaseContext")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                var user = context.Users.Add(new User()).Entity;
                var location =  context.Locations.Add(new Location()).Entity;
                var product = context.Products.Add(new Product
                {
                    Location = location
                }).Entity;
                context.Orders.Add(new Order()
                {
                    User = user,
                    Location = location
                });
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                var orders = context.Orders.ToList();
                Assert.Single(orders);
            }
        }
    }
}
