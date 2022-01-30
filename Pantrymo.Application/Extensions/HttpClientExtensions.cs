using Newtonsoft.Json;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Result<T>> GetJsonAsync<T>(this HttpClient client, string url)
            where T : new()
        {
            if (url == null)
                return Result.Failure<T>(new T());

            var json = await client.GetStringAsync(url)
                                   .DebugLogError();

            if (json == null)
                return Result.Failure<T>(new T());

            return Result.Success(JsonConvert.DeserializeObject<T>(json) ?? new T());
        }

        public static async Task<Result<T[]>> GetJsonArrayAsync<T>(this HttpClient client, string url)
        {
            if (url == null)
                return Result.Failure(new T[] { });

            var json = await client.GetStringAsync(url)
                                   .DebugLogError();

            if (json == null)
                return Result.Failure(new T[] { });

            return Result.Success(JsonConvert.DeserializeObject<T[]>(json) ?? new T[] { });

        }
    }
}
