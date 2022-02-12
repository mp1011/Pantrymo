namespace Pantrymo.Domain.Extensions
{
    public static class TypeExtensions
    {
        public static string GetModelName(this Type t)
        {
            var modelName = t.Name;
            if (modelName.StartsWith("I"))
                modelName = modelName.Substring(1);
            return modelName;
        }
    }
}
