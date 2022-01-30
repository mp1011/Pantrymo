using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pantrymo.Application.Models;

namespace Pantrymo.ServerInfrastructure
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
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<ComponentDiet> ComponentDiets { get; set; }
        public virtual DbSet<ComponentDietInfo> ComponentDietInfos { get; set; }
        public virtual DbSet<ComponentHierarchy> ComponentHierarchies { get; set; }
        public virtual DbSet<ComponentNegative> ComponentNegatives { get; set; }
        public virtual DbSet<ComponentNegativeRelation> ComponentNegativeRelations { get; set; }
        public virtual DbSet<ComponentsByCuisine> ComponentsByCuisines { get; set; }
        public virtual DbSet<ComponentsByCuisineCached> ComponentsByCuisineCacheds { get; set; }
        public virtual DbSet<ComponentsByCuisineWithTotal> ComponentsByCuisineWithTotals { get; set; }
        public virtual DbSet<ComponentsByCuisineWithoutOverall> ComponentsByCuisineWithoutOveralls { get; set; }
        public virtual DbSet<ComponentsByLevel> ComponentsByLevels { get; set; }
        public virtual DbSet<ComponentsWithAlternateName> ComponentsWithAlternateNames { get; set; }
        public virtual DbSet<ComponentsWithAncestor> ComponentsWithAncestors { get; set; }
        public virtual DbSet<Cuisine> Cuisines { get; set; }
        public virtual DbSet<CuisineHierarchy> CuisineHierarchies { get; set; }
        public virtual DbSet<CuisineLookup> CuisineLookups { get; set; }
        public virtual DbSet<IngredientMeasurement> IngredientMeasurements { get; set; }
        public virtual DbSet<IngredientText> IngredientTexts { get; set; }
        public virtual DbSet<LatestRecipeAudit> LatestRecipeAudits { get; set; }
        public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }
        public virtual DbSet<NavigationHint> NavigationHints { get; set; }
        public virtual DbSet<ParsedCuisine> ParsedCuisines { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeAudit> RecipeAudits { get; set; }
        public virtual DbSet<RecipeCountBySite> RecipeCountBySites { get; set; }
        public virtual DbSet<RecipeCountsByCuisine> RecipeCountsByCuisines { get; set; }
        public virtual DbSet<RecipeCountsBySiteAndCuisine> RecipeCountsBySiteAndCuisines { get; set; }
        public virtual DbSet<RecipeCountsBySiteAndCuisineWithHierarchy> RecipeCountsBySiteAndCuisineWithHierarchies { get; set; }
        public virtual DbSet<RecipeCountsBySiteAndCuisineWithTotal> RecipeCountsBySiteAndCuisineWithTotals { get; set; }
        public virtual DbSet<RecipeCuisine> RecipeCuisines { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeIngredientCount> RecipeIngredientCounts { get; set; }
        public virtual DbSet<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }
        public virtual DbSet<RecipeIngredientsDetail> RecipeIngredientsDetails { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
        public virtual DbSet<RecipeTrait> RecipeTraits { get; set; }
        public virtual DbSet<RecipeValidAndInvalidComponentCount> RecipeValidAndInvalidComponentCounts { get; set; }
        public virtual DbSet<RecipesWithCuisine> RecipesWithCuisines { get; set; }
        public virtual DbSet<RecipesWithoutCuisine> RecipesWithoutCuisines { get; set; }
        public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }
        public virtual DbSet<ScraperHint> ScraperHints { get; set; }
        public virtual DbSet<SearchFilter> SearchFilters { get; set; }
        public virtual DbSet<SearchFilterTrait> SearchFilterTraits { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SkipWord> SkipWords { get; set; }
        public virtual DbSet<TextOccurence> TextOccurences { get; set; }
        public virtual DbSet<Trait> Traits { get; set; }
        public virtual DbSet<TraitCount> TraitCounts { get; set; }
        public virtual DbSet<TraitValue> TraitValues { get; set; }
        public virtual DbSet<TraitsByRecipe> TraitsByRecipes { get; set; }
        public virtual DbSet<UnknownClassification> UnknownClassifications { get; set; }
        public virtual DbSet<UnknownClassificationsBySite> UnknownClassificationsBySites { get; set; }
        public virtual DbSet<UnknownTextCleaned> UnknownTextCleaneds { get; set; }
        public virtual DbSet<UnknownTextWord> UnknownTextWords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=PantryMoDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlternateComponentName>(entity =>
            {
                entity.Property(e => e.AlternateName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.AlternateComponentNames)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlternateComponentNames_Components");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Components")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentDiet>(entity =>
            {
                entity.HasIndex(e => e.ComponentId, "IX_ComponentDiets")
                    .IsUnique();

                entity.HasOne(d => d.Component)
                    .WithOne(p => p.ComponentDiet)
                    .HasForeignKey<ComponentDiet>(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponentDiets_Components");
            });

            modelBuilder.Entity<ComponentDietInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentDietInfo");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentHierarchy>(entity =>
            {
                entity.ToTable("ComponentHierarchy");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.ComponentHierarchies)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponentHierarchy_Components");
            });

            modelBuilder.Entity<ComponentNegative>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentNegatives");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NegativeComponent)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
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

            modelBuilder.Entity<ComponentsByCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsByCuisine");

                entity.Property(e => e.Component)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsByCuisineCached>(entity =>
            {
                entity.ToTable("ComponentsByCuisine_Cached");

                entity.Property(e => e.Cuisine)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsByCuisineWithTotal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsByCuisineWithTotals");

                entity.Property(e => e.Component)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsByCuisineWithoutOverall>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsByCuisine_WithoutOverall");

                entity.Property(e => e.Component)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsByLevel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsByLevel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsWithAlternateName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsWithAlternateNames");

                entity.Property(e => e.AlternateName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComponentsWithAncestor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ComponentsWithAncestors");

                entity.Property(e => e.AncestorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
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

                entity.HasOne(d => d.Cuisine)
                    .WithMany(p => p.CuisineHierarchies)
                    .HasForeignKey(d => d.CuisineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CusineHierarchy_Cuisines");
            });

            modelBuilder.Entity<CuisineLookup>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IngredientMeasurement>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("IngredientMeasurements");

                entity.Property(e => e.Measurement).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);
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

            modelBuilder.Entity<LatestRecipeAudit>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("LatestRecipeAudits");

                entity.Property(e => e.AuditDate).HasColumnType("datetime");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MeasurementType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NavigationHint>(entity =>
            {
                entity.Property(e => e.Pattern)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.NavigationHints)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NavigationHint_Sites");
            });

            modelBuilder.Entity<ParsedCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ParsedCuisines");

                entity.Property(e => e.ParsedCuisine1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Parsed Cuisine");

                entity.Property(e => e.ResolvedCuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Resolved Cuisine");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
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

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Recipe_Author");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Site");
            });

            modelBuilder.Entity<RecipeAudit>(entity =>
            {
                entity.Property(e => e.AuditDate).HasColumnType("datetime");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeAudits)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_RecipeAudits_Recipes");
            });

            modelBuilder.Entity<RecipeCountBySite>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCountBySite");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeCountsByCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCountsByCuisine");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeCountsBySiteAndCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCountsBySiteAndCuisine");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeCountsBySiteAndCuisineWithHierarchy>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCountsBySiteAndCuisineWithHierarchy");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine2)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine3)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine4)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cuisine5)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeCountsBySiteAndCuisineWithTotal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCountsBySiteAndCuisineWithTotals");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeCuisines");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");
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

            modelBuilder.Entity<RecipeIngredientCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeIngredientCounts");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeIngredientMeasurement>(entity =>
            {
                entity.ToTable("RecipeIngredientMeasurement");

                entity.Property(e => e.MeasurementAmount).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.IngredientText)
                    .WithMany(p => p.RecipeIngredientMeasurements)
                    .HasForeignKey(d => d.IngredientTextId)
                    .HasConstraintName("FK_RecipeIngredientMeasurement_IngredientText");

                entity.HasOne(d => d.MeasurementType)
                    .WithMany(p => p.RecipeIngredientMeasurements)
                    .HasForeignKey(d => d.MeasurementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredientMeasurement_MeasurementTypes");
            });

            modelBuilder.Entity<RecipeIngredientsDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeIngredientsDetail");

                entity.Property(e => e.Component)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IngredientText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ingredient Text");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipeStep>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeSteps)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeSteps_Recipes");
            });

            modelBuilder.Entity<RecipeTrait>(entity =>
            {
                entity.HasIndex(e => new { e.RecipeId, e.TraitValueId }, "IX_RecipeTraits")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TraitValueId, "IX_RecipeTraits_TraitValueId")
                    .HasFillFactor(80);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeTraits)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_RecipeTraits_Recipes");

                entity.HasOne(d => d.TraitValue)
                    .WithMany(p => p.RecipeTraits)
                    .HasForeignKey(d => d.TraitValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeTraits_TraitValues");
            });

            modelBuilder.Entity<RecipeValidAndInvalidComponentCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipeValidAndInvalidComponentCounts");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("recipe");

                entity.Property(e => e.Recipeid).HasColumnName("recipeid");
            });

            modelBuilder.Entity<RecipesWithCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipesWithCuisine");

                entity.Property(e => e.Cuisine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecipesWithoutCuisine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecipesWithoutCuisine");

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchemaVersion>(entity =>
            {
                entity.Property(e => e.Applied).HasColumnType("datetime");

                entity.Property(e => e.ScriptName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ScraperHint>(entity =>
            {
                entity.Property(e => e.Selector)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.ScraperHints)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScraperHints_Sites");
            });

            modelBuilder.Entity<SearchFilter>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SearchFilterTrait>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.SearchFilterId, e.TraitValueId }, "IX_SearchFilterTraits")
                    .IsUnique();

                entity.HasOne(d => d.SearchFilter)
                    .WithMany()
                    .HasForeignKey(d => d.SearchFilterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SearchFilterTraits_SearchFilters");

                entity.HasOne(d => d.TraitValue)
                    .WithMany()
                    .HasForeignKey(d => d.TraitValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SearchFilterTraits_TraitValues");
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

            modelBuilder.Entity<SkipWord>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TextOccurence>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trait>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TraitCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TraitCounts");

                entity.Property(e => e.Trait)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TraitValue>(entity =>
            {
                entity.HasIndex(e => new { e.TraitId, e.Name }, "IX_TraitValues")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Trait)
                    .WithMany(p => p.TraitValues)
                    .HasForeignKey(d => d.TraitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraitValues_Traits");
            });

            modelBuilder.Entity<TraitsByRecipe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TraitsByRecipe");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Trait)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnknownClassification>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UnknownClassifications");

                entity.Property(e => e.Component)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IngredientText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ingredient Text");

                entity.Property(e => e.Recipe)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnknownClassificationsBySite>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UnknownClassificationsBySite");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnknownTextCleaned>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UnknownTextCleaned");

                entity.Property(e => e.CleanedText).IsUnicode(false);

                entity.Property(e => e.IngredientText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ingredient Text");
            });

            modelBuilder.Entity<UnknownTextWord>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UnknownTextWords");

                entity.Property(e => e.Word).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
