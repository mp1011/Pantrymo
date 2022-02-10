namespace Pantrymo.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static T CheckNotNull<T>(this T item) where T:class
        {
            return item ?? throw new Exception($"Null value encountered for type {typeof(T).FullName}");
        }
    }
}
