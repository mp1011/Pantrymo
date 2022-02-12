#nullable disable

using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

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

        public async Task<Result<T[]>> GetJsonArrayAsync<T>(string url, object body)
        {
            var bodyJson = _jsonSerializer.Serialize(body);

            using var webClient = new HttpClient();
            if (url == null)
                return Result.Failure<T[]>(new NullReferenceException("url cannot be empty"));

            var result = await webClient.PostAsync(url, new StringContent(bodyJson, Encoding.UTF8, "application/json"))
                                        .AsResult(_errorHandler);

            if (result.Failure)
                return Result.Failure<T[]>(result.Error);

            if (result.Data.StatusCode != HttpStatusCode.OK)
                return Result.Failure<T[]>(new Exception($"{url} return HTTP Status {result.Data.StatusCode}"));

            var resultJson = await result.Data.Content
                .ReadAsStringAsync()
                .AsResult(_errorHandler);
                
            if(resultJson.Failure)
                return Result.Failure<T[]>(resultJson.Error);

            return Result.Success(_jsonSerializer.DeserializeArray<T>(resultJson.Data));
        }
    }
}
