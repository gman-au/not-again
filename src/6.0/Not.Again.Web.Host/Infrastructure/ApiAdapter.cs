using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Not.Again.Web.Host.Options;

namespace Not.Again.Web.Host.Infrastructure
{
    public class ApiAdapter : IApiAdapter
    {
        private readonly WebHostConfigurationOptions _options;
        private readonly ILogger<ApiAdapter> _logger;

        private readonly JsonSerializerOptions _serializerOptions =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public ApiAdapter(
            IOptions<WebHostConfigurationOptions> optionsAccessor,
            ILogger<ApiAdapter> logger)
        {
            _logger = logger;
            _options =
                optionsAccessor
                    .Value;
        }

        public async Task<TResponse> HttpPostAsync<TRequest, TResponse>(
            string path,
            TRequest request = default,
            CancellationToken cancellationToken = default)
        {
            var jsonString =
                JsonSerializer
                    .Serialize(request, _serializerOptions);

            var response =
                await
                    HttpSendAsync<TResponse>
                    (path,
                        (c, t) => c.PostAsync(
                            path,
                            new StringContent(jsonString, Encoding.Default, "application/json"),
                            t),
                        cancellationToken);

            return response;
        }

        public async Task<TResponse> HttpGetAsync<TResponse>(string path, CancellationToken cancellationToken = default)
        {
            var response =
                await
                    HttpSendAsync<TResponse>
                    (path,
                        (c, t) => c.GetAsync(path, t),
                        cancellationToken);

            return response;
        }

        private async Task<TResponse> HttpSendAsync<TResponse>(
            string path,
            Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> httpFunc,
            CancellationToken cancellationToken = default)
        {
            _logger
                .LogInformation("Sending API request: {path}", path);

            try
            {
                using var httpClient = new HttpClient();

                httpClient.BaseAddress =
                    new Uri(
                        _options.ApiEndpoint ??
                        throw new Exception(
                            "Configuration error: [WebHostConfigurationOptions -> ApiEndpoint] not defined; check configuration."));

                var httpResponse =
                    await
                        httpFunc(httpClient, cancellationToken);

                httpResponse
                    .EnsureSuccessStatusCode();

                var serializedResponse =
                    await
                        httpResponse
                            .Content
                            .ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

                return serializedResponse;
            }
            catch (Exception ex)
            {
                _logger
                    .LogError("Error sending API request: {path}: {message}", path, ex.Message);

                throw;
            }
        }
    }
}