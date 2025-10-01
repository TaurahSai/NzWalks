using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NzWalks.API.Data
{
    public class NzWalksAuthDbContext : IdentityDbContext
    {
        public NzWalksAuthDbContext(DbContextOptions<NzWalksAuthDbContext> dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                    Name = "Reader",
                    NormalizedName = "READER"
                },
                new IdentityRole
                {
                    Id = "b2c3d4e5-f678-90ab-cdef-234567890abc",
                    Name = "Writer",
                    NormalizedName = "WRITER"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
