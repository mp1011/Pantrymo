using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Extensions
{
    public static class IWithNameExtensions
    {
        public static Dictionary<string,T> ToDictionary<T>(this IEnumerable<T> list)
            where T:IWithName
        {
            return list.ToDictionary(k => k.Name, v => v);
        }
    }
}
