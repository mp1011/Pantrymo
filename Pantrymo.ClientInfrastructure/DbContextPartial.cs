using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;

namespace Pantrymo.ClientInfrastructure
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
            .Include(c => c.AlternateComponentNames);
            // add these lines once those tables are imported into sqlite
            //.Include(c => c.ComponentNegativeRelationComponents)
            //  .ThenInclude(c => c.NegativeComponent);

        IQueryable<AlternateComponentName> IDataContext.AlternateComponentNames => AlternateComponentNames;

        public async Task InsertAsync(Site[] records) => await Sites.AddRangeAsync(records);
        public async Task InsertAsync(Component[] records) => await Components.AddRangeAsync(records);
        public async Task InsertAsync(AlternateComponentName[] records) => await AlternateComponentNames.AddRangeAsync(records);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _settingsService.ConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .Ignore(e => e.ComponentHierarchies)
                .Ignore(e => e.ComponentNegativeRelationComponents)
                .Ignore(e => e.ComponentNegativeRelationNegativeComponents);
        }
    }

}
