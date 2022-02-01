namespace Pantrymo.Application.Models
{
    public class CachedData
    {
        public DateTime Expiration { get; }
        public object Data { get; }

        public bool IsExpired => Expiration < DateTime.Now;

        public CachedData(DateTime expiration, object data)
        {
            Expiration = expiration;
            Data = data;
        }
    }
}
