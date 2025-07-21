using NzWalks.API.Data;

namespace NzWalks.API.Repositories
{
    public class RegionsRepository : BaseRepository<Model.Domain.Region>, IRegionsRepository
    {
        public RegionsRepository(NzWalksDbContext dbContext) : base(dbContext)
        {
        }
    }
}
