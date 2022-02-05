using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;
using Pantrymo.SqlInfrastructure;

namespace Pantrymo.ClientInfrastructure
{
    public partial class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext(ISettingsService settingsService, DbContextOptions<SqliteDbContext> options)
          : base(settingsService, options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _settingsService.ConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FullHierarchy>()
              .HasNoKey();

            modelBuilder.Entity<RecipeSearchResult>()
               .HasKey(p => p.RecipeId);

            //modelBuilder.Entity<Component>()
            //   .Ignore(e => e.ComponentHierarchies)
            //   .Ignore(e => e.ComponentNegativeRelationComponents)
            //   .Ignore(e => e.ComponentNegativeRelationNegativeComponents);

            modelBuilder.Entity<AlternateComponentName>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AlternateName).IsRequired();

                entity.Property(e => e.LastModified).IsRequired();
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LastModified).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.Url).IsRequired();
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LastModified).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Url).IsRequired();
            });
        }
    }
}
