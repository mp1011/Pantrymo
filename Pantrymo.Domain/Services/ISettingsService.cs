namespace Pantrymo.Domain.Services
{
    public interface ISettingsService
    {
        string ConnectionString { get; }
        string Host { get; }
        string LocalDataFolder { get; }

        TimeSpan GetCacheDuration<T>();
    }
}
