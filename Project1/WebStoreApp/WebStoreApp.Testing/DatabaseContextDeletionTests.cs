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
    public class DatabaseContextDeletionTests
    {
        [Fact]
        public void DeleteUserTypesWithDatabaseContext()
        {
            // Arrange
            var options = SetUp("DeleteUsertypesWithDatabaseContext");

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                context.UserTypes.Remove(context.UserTypes.Include(userType => userType.Users).ThenInclude(user => user.UserInfo).First());
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Empty(context.UserTypes);
                Assert.Empty(context.Users);
                Assert.Empty(context.UserInfos);
            }
        }

        [Fact]
        public void DeleteUserWithDatabaseContext()
        {
            // Arrange
            var options = SetUp("DeleteUserWithDatabaseContext");

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                context.Users.Remove(context.Users.Include(user => user.UserInfo).First());
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Empty(context.Users);
                Assert.Empty(context.UserInfos);
            }
        }

        [Fact]
        public void DeleteLocationWithDatabaseContext()
        {
            // Arrange
            var options = SetUp("DeleteLocationWithDatabaseContext");

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                context.Locations.Remove(context.Locations.Include(location => location.Products).First());
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Empty(context.Locations);
                Assert.Empty(context.Products);
            }

        }

        [Fact]
        public void DeleteProductWithDatabaseContext()
        {
            var options = SetUp("DeleteProductWithDatabaseContext");

            using (var context = new WebStoreAppContext(options))
            {
                context.Products.Remove(context.Products.First());
                context.SaveChanges();
            }

            using (var context = new WebStoreAppContext(options))
            {
                Assert.Empty(context.Products);
            }
        }

        [Fact]
        public void DeleteOrderWithDatabaseContext()
        {
            var options = SetUp("DeleteOrderWithDatabaseContext");

            using (var context = new WebStoreAppContext(options))
            {
                context.Orders.Remove(context.Orders.First());
                context.SaveChanges();
            }

            using (var context = new WebStoreAppContext(options))
            {
                Assert.Empty(context.Orders);
            }
        }

        public DbContextOptions<WebStoreAppContext> SetUp(string databaseName)
        {
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            using (var context = new WebStoreAppContext(options))
            {
                context.UserTypes.Add(new UserType());
                context.SaveChanges();
                context.Users.Add(new User
                {
                    UserType = context.UserTypes.First(),
                    UserInfo = new UserInfo()
                });
                context.Locations.Add(new Location());
                context.SaveChanges();
                var product = context.Products.Add(new Product
                {
                    Location = context.Locations.First()
                });
                context.SaveChanges();
                var order = context.Orders.Add(new Order
                {
                    User = context.Users.First(),
                    Location = context.Locations.First(),
                    OrderInfo = new OrderInfo()
                });
                context.SaveChanges();
            }

            return options;
        }
    }
}