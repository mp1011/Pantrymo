using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Extensions
{
    public static class IWithLastModifiedDateExtensions
    {
        public static DateTime GetLatestModifiedDate<T>(this IQueryable<T> query)
             where T : IWithLastModifiedDate
        {
            if (!query.Any())
                return new DateTime(1990, 1, 1);

            return query.Max(p => p.LastModified);
        }

        public static async Task<Result<T[]>> GetByDateAsync<T>(this IQueryable<T> query, DateTime dateFrom)
          where T : IWithLastModifiedDate
        {
            return await Task.Run(() => query
                    .Where(p => p.LastModified > dateFrom)
                    .OrderBy(p => p.LastModified)
                    .ToArray()).AsResult();
        }

        public static IEnumerable<RecordUpdateTimestamp> GetRecordUpdateTimestamps<T>(this IQueryable<T> query)
            where T:IWithId,IWithLastModifiedDate
        {
            return query.Select(p => new RecordUpdateTimestamp(p.Id, p.LastModified));
        }
    }
}
