using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.ServerInfrastructure
{
    public partial class PantryMoDBContext : IDataContext
    {
        private readonly ISettingsService _settingsService;

        public PantryMoDBContext(ISettingsService settingsService, DbContextOptions<PantryMoDBContext> options)
          : base(options)
        {
            _settingsService = settingsService;
        }

        IQueryable<Site> IDataContext.Sites => Sites;
        IQueryable<Component> IDataContext.Components => Components
            .Include(c => c.AlternateComponentNames)
            .Include(c => c.ComponentNegativeRelationComponents)
                .ThenInclude(c => c.NegativeComponent);

        IQueryable<AlternateComponentName> IDataContext.AlternateComponentNames => AlternateComponentNames;

        public virtual DbSet<FullHierarchy> FullHierarchy { get; set; }

        public async Task InsertAsync(Site[] records) => await Sites.AddRangeAsync(records);
        public async Task InsertAsync(Component[] records) => await Components.AddRangeAsync(records);
        public async Task InsertAsync(AlternateComponentName[] records) => await AlternateComponentNames.AddRangeAsync(records);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _settingsService.ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComponentHierarchy>()
                .Ignore(p=>p.HierarchyId);

            modelBuilder.Entity<CuisineHierarchy>()
                .Ignore(p => p.HierarchyId);

            modelBuilder.Entity<FullHierarchy>()
                .HasNoKey();
        }
    }
}
