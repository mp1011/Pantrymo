namespace Pantrymo.Domain.Models
{
    public interface IBaseDataContext
    {
        IQueryable<T> GetQuery<T>();

        Task Save<T>(params T[] records) where T : IWithId, IWithLastModifiedDate;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        string GetQueryString<T>(IQueryable<T> query);
    }
}
