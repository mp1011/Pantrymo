using Newtonsoft.Json;

namespace Pantrymo.Application.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> GetJsonAsync<T>(this HttpClient client, string url)
        {
            var json = await client.GetStringAsync(url)
                                   .DebugLogError();

            if (json == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
