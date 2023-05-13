using Pronia.Models;
using Pronia.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Database
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }

        public DbSet<Info> FAQ { get; set; } = null!;

        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product>  Products { get; set; } = null!;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }
    }

}
