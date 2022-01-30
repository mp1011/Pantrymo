using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;

namespace Pantrymo.ServerInfrastructure
{
    public partial class PantryMoDBContext : IDataContext
    {
        IQueryable<Site> IDataContext.Sites => Sites;
        IQueryable<Component> IDataContext.Components => Components;
        IQueryable<AlternateComponentName> IDataContext.AlternateComponentNames => AlternateComponentNames;

        public virtual DbSet<FullHierarchy> FullHierarchy { get; set; }

        public async Task InsertAsync(Site[] records) => await Sites.AddRangeAsync(records);
        public async Task InsertAsync(Component[] records) => await Components.AddRangeAsync(records);
        public async Task InsertAsync(AlternateComponentName[] records) => await AlternateComponentNames.AddRangeAsync(records);

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
