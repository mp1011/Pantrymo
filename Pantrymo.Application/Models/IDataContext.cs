namespace Pantrymo.Application.Models
{
    public interface IDataContext
    {
        IQueryable<Site> Sites { get; }

        Site[] ExecuteSQLSites(FormattableString sql);

        Task InsertAsync(Site[] sites);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
