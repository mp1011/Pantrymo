using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

        public virtual DbSet<AlternateComponentName> AlternateComponentNames { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }
        public virtual DbSet<Site> Sites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
