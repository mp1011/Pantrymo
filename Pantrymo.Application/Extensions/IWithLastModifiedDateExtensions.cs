using Pantrymo.Application.Models;

namespace Pantrymo.Application.Extensions
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

        public static T[] GetByDate<T>(this IQueryable<T> query, DateTime dateFrom)
            where T:IWithLastModifiedDate
        {
            return query
                    .Where(p => p.LastModified > dateFrom)
                    .OrderBy(p => p.LastModified)
                    .ToArray();
        }

        public static async Task<T[]> GetByDateAsync<T>(this IQueryable<T> query, DateTime dateFrom)
          where T : IWithLastModifiedDate
        {
            return await Task.Run(()=> query
                    .Where(p => p.LastModified > dateFrom)
                    .OrderBy(p => p.LastModified)
                    .ToArray());
        }
    }
}
