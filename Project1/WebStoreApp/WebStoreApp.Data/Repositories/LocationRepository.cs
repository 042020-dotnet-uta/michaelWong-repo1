using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;
using WebStoreApp.Domain.Interfaces;

namespace WebStoreApp.Data.Repository
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(WebStoreAppContext context)
            : base(context)
        {

        }

        public async override Task<IEnumerable<Location>> All()
        {
            return await base.Get(null, locations => locations.OrderBy(location => location.Name));
        }
    }
}