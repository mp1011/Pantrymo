#nullable disable

using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;
using Pantrymo.SqlInfrastructure;

namespace Pantrymo.ServerInfrastructure
{
    public partial class SqlServerDbContext : BaseDbContext
    {
        public SqlServerDbContext(ISettingsService settingsService, IObjectMapper objectMapper, DbContextOptions<SqlServerDbContext> options)
          : base(settingsService, objectMapper, options)
        {
        }
        public virtual DbSet<ComponentHierarchy> ComponentHierarchies { get; set; }
        public virtual DbSet<CuisineHierarchy> CuisineHierarchies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _settingsService.ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FullHierarchy>().HasNoKey();
            modelBuilder.Entity<RecipeSearchResult>().HasNoKey();

            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable("Components");
                entity.HasIndex(e => e.Name, "IX_Components")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentHierarchy>(entity =>
            {
                entity.ToTable("ComponentHierarchy");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                //entity.HasOne(d => d.Component)
                //    .WithMany(p => p.ComponentHierarchies)
                //    .HasForeignKey(d => d.ComponentId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_ComponentHierarchy_Components");
            });

            modelBuilder.Entity<ComponentNegativeRelation>(entity =>
            {
                entity.HasIndex(e => new { e.ComponentId, e.NegativeComponentId }, "IX_ComponentNegativeRelations")
                    .IsUnique();

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.ComponentNegativeRelationComponents)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponentNegativeRelations_Components");

                entity.HasOne(d => d.NegativeComponent)
                    .WithMany(p => p.ComponentNegativeRelationNegativeComponents)
                    .HasForeignKey(d => d.NegativeComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponentNegativeRelations_Components1");

            });

            modelBuilder.Entity<Cuisine>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Cuisines")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CuisineHierarchy>(entity =>
            {
                entity.ToTable("CuisineHierarchy");

                //entity.HasOne(d => d.Cuisine)
                //    .WithMany(p => p.CuisineHierarchies)
                //    .HasForeignKey(d => d.CuisineId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_CusineHierarchy_Cuisines");
            });

            modelBuilder.Entity<IngredientText>(entity =>
            {
                entity.ToTable("IngredientText");

                entity.HasIndex(e => e.RecipeId, "IX_IngredientText_RecipeId");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.IngredientTexts)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_IngredientText_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipes");

                entity.HasIndex(e => e.Id, "IX_Recipes")
                    .HasFillFactor(80);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Author);
                entity.HasOne(d => d.Site);
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasIndex(e => e.ComponentId, "IX_RecipeIngredient_ComponentId")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ComponentId, "IX_RecipeIngredients_ComponentId")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TextId, "IX_RecipeIngredients_TextId")
                    .HasFillFactor(80);

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredients_Components");

                entity.HasOne(d => d.Text)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.TextId)
                    .HasConstraintName("FK_RecipeIngredients_IngredientText");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }

    }
}
