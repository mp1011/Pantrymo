using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;

namespace Pantrymo.ClientInfrastructure
{
    public partial class PantryMoDBContext : DbContext
    {
        public PantryMoDBContext()
        {
        }

        public PantryMoDBContext(DbContextOptions<PantryMoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }
        public virtual DbSet<Site> Sites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=C:\\ProgramData\\Pantrymo\\PantryMoDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
