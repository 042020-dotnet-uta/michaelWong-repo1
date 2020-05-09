using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Data;
using WebStoreApp.Domain;
using System.Linq;

namespace WebStoreApp.Testing
{
    public class DatabaseUnitTests
    {
        [Fact]
        public void AddRecordsToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "CreateDatabase")
                .Options;

            // Act
            using (var context = new WebStoreAppContext(options))
            {
                // Adds Admin type to Database
                context.UserTypes.Add(new UserType { Name = "Admin", Description = "" });
                // Add Customer type to Database
                context.UserTypes.Add(new UserType { Name = "Customer", Description = "" });
                context.SaveChanges();
            }

            // Assert
            using (var context = new WebStoreAppContext(options))
            {
                Assert.Equal(2, context.UserTypes.ToList().Count);
            }
        }
    }
}
