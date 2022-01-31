using Microsoft.Extensions.Configuration;

namespace Pantrymo.Application.Services
{
    public class SettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("PantrymoDb");

        public string Host => _configuration[nameof(Host)];

        public string LocalDataFolder => _configuration[nameof(LocalDataFolder)];
    }
}
