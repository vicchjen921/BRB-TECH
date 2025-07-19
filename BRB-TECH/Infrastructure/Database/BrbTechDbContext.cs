using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database
{
    public class BrbTechDbContext : DbContext
    {
        public BrbTechDbContext(DbContextOptions<BrbTechDbContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<DocumentTransfer> DocumentTransfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(k => k.Id);
                e.HasIndex(k => k.Amount);
                e.HasIndex(k => k.CategoryId);
                e.HasIndex(k => k.Type);
                e.HasIndex(k => k.CreatedAt);
                e.HasIndex(k => new { k.UserId, k.State});
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(k => k.Id);
                e.HasIndex(k => new { k.UserId, k.State });
            });

            modelBuilder.Entity<AuditLog>(e =>
            {
                e.HasKey(o => o.Id);
            });

            modelBuilder.Entity<DocumentTransfer>(e =>
            {
                e.HasKey(d => d.Id);
                e.HasIndex(d => new { d.State });
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class BrbTechDbContextFactory : IDesignTimeDbContextFactory<BrbTechDbContext>
    {
        private const string ConnectionString = "Server=localhost;Database=BrbTech;User Id=sa;Password=sa;MultipleActiveResultSets=True;TrustServerCertificate=True;";

        public BrbTechDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<BrbTechDbContext> optionsBuilder = new DbContextOptionsBuilder<BrbTechDbContext>()
                .UseSqlServer(ConnectionString);

            return new BrbTechDbContext(optionsBuilder.Options);
        }
    }
}
