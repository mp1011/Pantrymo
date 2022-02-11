#nullable disable

using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using System.Net.NetworkInformation;

namespace Pantrymo.Domain.Services
{
    public class HttpService
    {
        private readonly CustomJsonSerializer _jsonSerializer;
        private readonly IExceptionHandler _errorHandler;

        public HttpService(CustomJsonSerializer jsonSerializer, IExceptionHandler errorHandler)
        {
            _jsonSerializer = jsonSerializer;
            _errorHandler = errorHandler;
        }
        public bool HasInternet() => NetworkInterface.GetIsNetworkAvailable();

        public async Task<Result<T>> GetJsonAsync<T>(string url)
                   where T : class, new()
        {
            using var webClient = new HttpClient();
            if (url == null)
                return Result.Failure<T>(new NullReferenceException("url cannot be empty"));

            var jsonResult = await webClient.GetStringAsync(url)
                                            .AsResult()
                                            .HandleError(_errorHandler);

            if (jsonResult.Failure)
                return Result.Failure<T>(jsonResult.Error);
            
            if(jsonResult.Data == null)
                return Result.Failure<T>(new NullReferenceException($"request to {url} did not return a value"));

            return Result.Success(_jsonSerializer.Deserialize<T>(jsonResult.Data));
        }

        public async Task<Result<T[]>> GetJsonArrayAsync<T>(string url)
        {
            using var webClient = new HttpClient();
            if (url == null)
                return Result.Failure<T[]>(new NullReferenceException("url cannot be empty"));

            var jsonResult = await webClient.GetStringAsync(url)
                                            .AsResult()
                                            .HandleError(_errorHandler);

            if (jsonResult.Failure)
                return Result.Failure<T[]>(jsonResult.Error);

            return Result.Success(_jsonSerializer.DeserializeArray<T>(jsonResult.Data));
        }
    }
}
