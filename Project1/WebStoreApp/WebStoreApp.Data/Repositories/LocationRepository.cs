using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;

namespace WebStoreApp.Data.Repository
{
    public class LocationRepository : BaseRepository<Location>
    {
        public LocationRepository(WebStoreAppContext context)
            : base(context)
        {

        }
    }
}