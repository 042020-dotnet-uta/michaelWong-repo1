using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using WebStoreApp.Data;
using WebStoreApp.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WebStoreApp.Testing
{
    public partial class UnitOfWorkTests
    {
        [Fact]
        public async Task InsertOrderTest()
        {
            // Arrange
            var options = SetUp("InsertOrderTest");
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.UserRepository.Insert(new User
                {
                    UserType = new UserType()
                });
                await unitOfWork.ProductRepository.Insert(new Product
                {
                    Location = new Location()
                });
                await unitOfWork.Save();
            }

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                var user = (await unitOfWork.UserRepository.All()).First();
                var location = (await unitOfWork.LocationRepository.All()).First();
                await unitOfWork.OrderRepository.Insert(new Order
                {
                    Location = location,
                    User = user
                });
                await unitOfWork.Save();
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                Assert.Single(await unitOfWork.OrderRepository.All());
            }
        }

        [Fact]
        public async Task OrderHistoryTests()
        {
            // Arrange
            var options = SetUp("OrderHistoryTests");
            User user; Location location;
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                user = await unitOfWork.UserRepository.Insert(new User
                {
                    UserType = new UserType()
                });
                location = await unitOfWork.LocationRepository.Insert(new Location());
                await unitOfWork.OrderRepository.Insert(new Order
                {
                    User = user,
                    Location = location
                });
                await unitOfWork.Save();
            }
            IEnumerable<Order> byUser; IEnumerable<Order> byLocation;

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                byLocation = await unitOfWork.OrderRepository.GetByLocation(location.Id);
                byUser = await unitOfWork.OrderRepository.GetByUser(user.Id);
            }

            // Assert
            Assert.Single(byUser);
            Assert.Single(byLocation);
        }
    }
}