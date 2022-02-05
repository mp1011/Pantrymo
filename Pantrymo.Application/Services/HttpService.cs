#nullable disable

using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public class HttpService
    {
        private readonly CustomJsonSerializer _jsonSerializer;

        public HttpService(CustomJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public async Task<Result<T>> GetJsonAsync<T>(string url)
                   where T : class, new()
        {
            using var webClient = new HttpClient();
            if (url == null)
                return Result.Failure<T>(new T());

            var json = await webClient.GetStringAsync(url)
                                   .DebugLogError();

            if (json == null)
                return Result.Failure<T>(new T());

            return Result.Success(_jsonSerializer.Deserialize<T>(json));
        }

        public async Task<Result<T[]>> GetJsonArrayAsync<T>(string url)
        {
            using var webClient = new HttpClient();
            if (url == null)
                return Result.Failure(new T[] { });

            var json = await webClient.GetStringAsync(url)
                                   .DebugLogError();

            if (json == null)
                return Result.Failure(new T[] { });

            return Result.Success(_jsonSerializer.DeserializeArray<T>(json));
        }
    }
}
