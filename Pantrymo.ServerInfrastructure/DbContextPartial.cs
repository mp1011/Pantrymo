using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;

namespace Pantrymo.ServerInfrastructure
{
    public partial class PantryMoDBContext : IDataContext
    {
        IQueryable<Site> IDataContext.Sites => Sites;

        public Site[] ExecuteSQLSites(FormattableString sql) =>
                 Sites.FromSqlInterpolated(sql)
                .ToArray();

        public async Task InsertAsync(Site[] sites) => await Sites.AddRangeAsync(sites);

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CuisineHierarchy>(entity =>
        //    {
        //        entity.ToTable("CuisineHierarchy");

        //        entity.Property(e => e.HierarchyId).IsRequired();
        //    });
        //}
    }
}
