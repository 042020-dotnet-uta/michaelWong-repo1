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
        public async Task InsertUserTypeTest()
        {
            // Arrange
            var options = SetUp("InsertUserTypeTest");

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.UserTypeRepository.Insert(new UserType());
                await unitOfWork.Save();
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                Assert.Single(await unitOfWork.UserTypeRepository.All());
            }
        }

        [Fact]
        public async Task GetByNameTest()
        {
            // Arrange
            var options = SetUp("GetByNameTest");
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                await unitOfWork.UserTypeRepository.Insert(new UserType
                {
                    Name = "Test"
                });
                await unitOfWork.Save();
            }
            UserType userType;

            // Act
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                userType = await unitOfWork.UserTypeRepository.GetByName("Test");
            }

            // Assert
            using (var unitOfWork = new UnitOfWork(new WebStoreAppContext(options)))
            {
                Assert.NotNull(userType);
            }
        }
    }
}