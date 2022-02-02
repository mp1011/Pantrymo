using Newtonsoft.Json;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;

namespace Pantrymo.ClientInfrastructure.Services
{
    public class LocalStorage : ILocalStorage
    {
        private readonly DirectoryInfo _dataFolder;

        public LocalStorage(ISettingsService settingsService)
        {
            _dataFolder = new DirectoryInfo(settingsService.LocalDataFolder);
        }

        private FileInfo GetLocalFile<T>()
        {
            var fileName = typeof(T).IsArray
               ? $"{typeof(T).GetElementType().Name}Array.json"
               : $"{typeof(T).Name}.json";

            return new FileInfo($@"{_dataFolder.FullName}\{fileName}");
        }

        public async Task<T?> Get<T>()
        {
            var file = GetLocalFile<T>();
            if (!file.Exists)
                return default;

            var json = await File.ReadAllTextAsync(file.FullName);
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task Set<T>(T value)
        {
            var file = GetLocalFile<T>();
            if (file.Exists)
                file.Delete();

            var json = JsonConvert.SerializeObject(value);
            await File.WriteAllTextAsync(file.FullName, json);
        }
    }
}
