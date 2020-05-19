using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Data.Repository;
using WebStoreApp.Domain.Interfaces;
using WebStoreApp.Domain;
using WebStoreApp.Data;

namespace WebStoreApp.Testing
{
    public partial class GenericRepositoryTest
    {
        [Fact]
        public async Task InsertTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: "InsertTest")
                .Options;
            var mockRepo = new Mock<IRepository<User>>();

            // Act
            
        }
    }
}