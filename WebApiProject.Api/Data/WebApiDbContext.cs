using Microsoft.EntityFrameworkCore;
using WebApiProject.Api.Models.Domain;

namespace WebApiProject.Api.Data
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var difficulties = new List<Difficulty>() 
            { 
                new Difficulty()
                {
                    Id = Guid.NewGuid(),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.NewGuid(),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.NewGuid(),
                    Name = "Hard"
                },
            };

            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var regions = new List<Region>() 
            { 
                new Region() 
                {
                    Id = Guid.NewGuid(),
                    Code = "MX",
                    Name = "Mexico",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Flag_of_Mexico.svg/1200px-Flag_of_Mexico.svg.png"
                },
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "US",
                    Name = "United States",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a4/Flag_of_the_United_States.svg/2560px-Flag_of_the_United_States.svg.png"
                },
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "TW",
                    Name = "Taiwan",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Flag_of_the_Republic_of_China.svg/800px-Flag_of_the_Republic_of_China.svg.png"
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
