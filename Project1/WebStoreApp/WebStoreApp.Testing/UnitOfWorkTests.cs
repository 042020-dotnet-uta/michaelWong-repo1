using Microsoft.EntityFrameworkCore;
using WebStoreApp.Data;

namespace WebStoreApp.Testing
{
    public partial class UnitOfWorkTests
    {
        public DbContextOptions<WebStoreAppContext> SetUp(string databaseName)
        {
            var options = new DbContextOptionsBuilder<WebStoreAppContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return options;
        }
    }
}