namespace Pantrymo.Domain.Extensions
{
    public static class TypeExtensions
    {
        public static string GetModelName(this Type t)
        {
            var modelName = t.Name;
            if (modelName.StartsWith("I"))
                modelName = modelName.Substring(1);

            if (modelName.EndsWith("DTO"))
                modelName = modelName.Substring(0, modelName.LastIndexOf("DTO"));

            if (modelName.EndsWith("Detail"))
                modelName = modelName.Substring(0, modelName.LastIndexOf("Detail"));

            return modelName;
        }
    }
}
