namespace Pantrymo.Domain.Services
{
    public interface ILocalStorage
    {
        Task<T?> Get<T>();

        Task Set<T>(T value);
    }
}
