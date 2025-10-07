using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiProject.Api.Data
{
    public class WebApiAuthDbContext : IdentityDbContext
    {
        public WebApiAuthDbContext(DbContextOptions<WebApiAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readOnly = "27a8190b-b79b-4cc3-8aa5-fec1a8a5ddb6";
            var readWrite = "2341f2ad-cd2a-4b1d-a141-e2a718c618a9";

            var roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = readOnly,
                    ConcurrencyStamp = readOnly,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = readWrite,
                    ConcurrencyStamp = readWrite,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
