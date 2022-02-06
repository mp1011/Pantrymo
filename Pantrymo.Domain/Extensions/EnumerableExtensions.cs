namespace Pantrymo.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static void SplitUp<T>(this IEnumerable<T> list, Predicate<T> condition, 
            out ICollection<T> trueGroup, out ICollection<T> falseGroup)
        {
            trueGroup = new List<T>();
            falseGroup = new List<T>();

            foreach(var item in list)
            {
                if (condition(item))
                    trueGroup.Add(item);
                else
                    falseGroup.Add(item);
            }
        }
    }
}
