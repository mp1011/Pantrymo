using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Pantrymo.Domain.Services
{
    public class CustomJsonSerializer
    {
        private readonly Dictionary<Type, Type> _interfaceImplementations;

        public CustomJsonSerializer(Assembly interfacesAssembly, Assembly implementationsAssembly)
        {
            var interfaceTypes = interfacesAssembly
                .GetTypes()
                .Where(p => p.IsPublic && p.IsInterface)
                .ToDictionary(k => k.Name, v => v);


            _interfaceImplementations=new Dictionary<Type, Type>();
            foreach(var type in implementationsAssembly.GetTypes())
            {
                var typeInfo = type.GetTypeInfo();
                foreach(var implementedInterface in typeInfo.ImplementedInterfaces)
                {
                    if (interfaceTypes.ContainsKey(implementedInterface.Name) && !_interfaceImplementations.ContainsKey(implementedInterface))
                        _interfaceImplementations[implementedInterface] = type;
                }
            }
        }

        private Type GetDeserializeType(Type type)
        {
            if (type.IsInterface)
                return GetImplementingType(type);
            else
                return type;
        }

        public string Serialize(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public T? Deserialize<T>(string json)
            where T: class
        {
            
            var result = JsonConvert.DeserializeObject(json, GetDeserializeType(typeof(T)));
            return result as T;
        }

        private Type GetImplementingType(Type interfaceType)
        {
            return _interfaceImplementations.GetValueOrDefault(interfaceType) ?? throw new Exception($"There is no implementation for type {interfaceType.Name}");
        }

        public T[] DeserializeArray<T>(string json)
        {
            var arrayType = GetDeserializeType(typeof(T)).MakeArrayType();
            var result = (JsonConvert.DeserializeObject(json, arrayType) as T[]) ?? new T[] { };
            return result;

        }
    }


    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly Type[] _interfaceTypes;

        public CustomContractResolver(Assembly modelsAssembly)
        {
            _interfaceTypes = modelsAssembly.GetTypes()
                .Where(p => p.IsInterface)
                .ToArray();
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            if(type.IsInterface)
                return base.CreateProperties(type, memberSerialization);

            var implementedInterfaces = type
                .GetInterfaces()
                .Intersect(_interfaceTypes)
                .ToArray();

            if(!implementedInterfaces.Any())
                return base.CreateProperties(type, memberSerialization);

            var typeToUse = implementedInterfaces.FirstOrDefault(p => p.Name.EndsWith("DTO"))
                ?? implementedInterfaces.FirstOrDefault(p => !p.Name.Contains("Detail"))
                ?? type;

            return base.CreateProperties(typeToUse, memberSerialization);
        }
    }
}
