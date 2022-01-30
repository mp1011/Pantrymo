using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;

namespace Pantrymo.ClientInfrastructure
{
    public partial class PantryMoDBContext : IDataContext
    {
        IQueryable<Site> IDataContext.Sites => Sites;
        IQueryable<Component> IDataContext.Components => Components;
        IQueryable<AlternateComponentName> IDataContext.AlternateComponentNames => AlternateComponentNames;

        public async Task InsertAsync(Site[] records) => await Sites.AddRangeAsync(records);
        public async Task InsertAsync(Component[] records) => await Components.AddRangeAsync(records);
        public async Task InsertAsync(AlternateComponentName[] records) => await AlternateComponentNames.AddRangeAsync(records);

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .Ignore(e => e.ComponentHierarchies)
                .Ignore(e => e.ComponentNegativeRelationComponents)
                .Ignore(e => e.ComponentNegativeRelationNegativeComponents);
        }
    }

}
