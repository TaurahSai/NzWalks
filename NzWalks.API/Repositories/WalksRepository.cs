using NzWalks.API.Data;

namespace NzWalks.API.Repositories
{
    public class WalksRepository : BaseRepository<Model.Domain.Walk>, IWalksRepository
    {
        public WalksRepository(NzWalksDbContext dbContext) : base(dbContext)
        {
        }
    }
}
