using Microsoft.Extensions.Configuration;
using Pantrymo.Application.Extensions;

namespace Pantrymo.Application.Services
{
    public interface ISettingsService
    {
        string ConnectionString { get; }
        string Host { get; }
        string LocalDataFolder { get; }

        TimeSpan GetCacheDuration<T>();
    }

    public class SettingsService : ISettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("PantrymoDb");

        public string Host => _configuration[nameof(Host)];

        public string LocalDataFolder => _configuration[nameof(LocalDataFolder)];

        public TimeSpan GetCacheDuration<T>()
        {
            var typeConfig = _configuration[$"CacheDurationSeconds_{typeof(T).Name}"];
            if (string.IsNullOrEmpty(typeConfig))
                typeConfig = _configuration[$"CacheDurationSeconds"];

            int seconds = typeConfig.TryParseInt();
            return TimeSpan.FromSeconds(seconds);
        }
    }
}
