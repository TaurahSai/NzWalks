using Microsoft.EntityFrameworkCore;
using NzWalks.API.Model.Domain;

namespace NzWalks.API.Data
{
    public class NzWalksDbContext : DbContext
    {
        public NzWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed data for Difficulties
            modelBuilder.Entity<Difficulty>().HasData(
                new Difficulty { Id = new Guid("b10feddb-72d1-4bdb-9a80-806d873b7d63"), Name = "Easy" },
                new Difficulty { Id = new Guid("cc3ce127-c947-43a3-8fa9-27663247daa3"), Name = "Hard" },
                new Difficulty { Id = new Guid("e6b1b599-03c5-4d6f-8e0a-b93289f4650e"), Name = "Medium" }
            );
            // Seed data for Regions
            modelBuilder.Entity<Region>().HasData(
                new Region { Id = new Guid("bc1e31f0-ec44-4980-bb40-d470e4f5ed0e"), Code = "NZ-001", Name = "North Island", RegionImageUrl = "https://example.com/north-island.jpg" },
                new Region { Id = new Guid("28c791cd-4e28-4a03-b4a7-78e80387fa91"), Code = "NZ-002", Name = "South Island", RegionImageUrl = "https://example.com/south-island.jpg" }
            );
        }
    }
}
