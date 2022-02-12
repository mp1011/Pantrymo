using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Extensions
{
    public static class IWithIdExtensions
    {
        public static int NextId<T>(this IQueryable<T> query)
            where T:IWithId
        {
            if (!query.Any())
                return 1;

            return query.Select(p => p.Id).Max() + 1;
        }
    }
}
