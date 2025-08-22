using System;
using System.Diagnostics;
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
            TRequest request = default,
            CancellationToken cancellationToken = default)
        {
            _logger
                .LogInformation("Sending Test Runs API request");

            try
            {
                using var httpClient = new HttpClient();

                httpClient.BaseAddress =
                    new Uri(
                        _options.ApiEndpoint ??
                        throw new Exception(
                            "Configuration error: [WebHostConfigurationOptions -> ApiEndpoint] not defined; check configuration."));

                var jsonString =
                    JsonSerializer
                        .Serialize(request, _serializerOptions);

                var httpResponse =
                    await
                        httpClient
                            .PostAsync(
                                "/Diagnostic/RunCheck",
                                new StringContent(jsonString, Encoding.Default, "application/json"),
                                cancellationToken
                            );

                var serializedResponse =
                    await
                        httpResponse
                            .Content
                            .ReadFromJsonAsync<TResponse>(_serializerOptions, cancellationToken);

                return serializedResponse;
            }
            catch (Exception ex)
            {
                _logger
                    .LogError("Error sending Test Runs API request: {message}",  ex.Message);

                throw;
            }
        }
    }
}