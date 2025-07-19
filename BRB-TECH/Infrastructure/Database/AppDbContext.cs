using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            private const string ConnectionString = "Server=localhost;Database=BrbTech;User Id=sa;Password=sa;MultipleActiveResultSets=True;TrustServerCertificate=True;";

            public AppDbContext CreateDbContext(string[] args)
            {
                DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(ConnectionString);

                return new AppDbContext(optionsBuilder.Options);
            }
        }
    }
}
