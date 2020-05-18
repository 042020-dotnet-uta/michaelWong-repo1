using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WebStoreApp.Data;
using WebStoreApp.Domain;

namespace WebStoreApp.Testing
{
    public partial class UnitOfWorkTests
    {
        [Fact]
        public async Task InsertProductTest()
        {
            // Arrange
            var options = SetUp("InsertProductTest");

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.ProductRepository.Insert(new Product
                {
                    Location = new Location()
                });
                await unitOfWork.Save();
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                Assert.Single(await unitOfWork.ProductRepository.All());
            }
        }

        [Fact]
        public async Task GetByLocationTest()
        {
            // Arrange
            var options = SetUp("GetByLocationTest");
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.ProductRepository.Insert(new Product
                {
                    Location = new Location()
                });
                await unitOfWork.Save();
            }
            Product product;

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                var location = (await unitOfWork.LocationRepository.All()).ToList().First();
                product = (await unitOfWork.ProductRepository.GetByLocation(location.Id)).ToList().First();
            }

            // Assert
            Assert.NotNull(product);
        }
    }
}