using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Infrastructure
{
    public static class ApiAdapter
    {
        private const string RunCheckEndpoint = "/Diagnostic/RunCheck";
        private const string SubmitResultRequest = "/Diagnostic/ReportResult";
        
        public static async Task<bool> RunCheckAsync(
            RunCheckRequest runCheckRequest,
            string notAgainBaseUrl,
            Action<string> logAction
        )
        {
            try
            {
                var handler = GetCertIgnoreHandler();

                using var client = new HttpClient(handler);
                
                var uri = new Uri(notAgainBaseUrl);
                client.BaseAddress = uri;

                var result =
                    await
                        client
                            .PostAsJsonAsync(
                                RunCheckEndpoint,
                                runCheckRequest
                            );

                if (result.StatusCode == HttpStatusCode.AlreadyReported)
                    return true;
            }
            catch (HttpRequestException ex)
            {
                logAction("Warning - there was an error when attempting to connect to the API; please check your configuration.");
            }

            logAction($"Submitted check request for test ID [{runCheckRequest?.TestDetails?.Id}]");
            
            return false;
        }

        public static async Task SubmitResultAsync(
            SubmitResultRequest submitResultRequest,
            string notAgainBaseUrl,
            Action<string> logAction
        )
        {
            try
            {
                var handler = GetCertIgnoreHandler();
                
                using var client = new HttpClient(handler);
                
                var uri = new Uri(notAgainBaseUrl);
                client.BaseAddress = uri;

                await
                    client
                        .PostAsJsonAsync(
                            SubmitResultRequest,
                            submitResultRequest
                        );
                    
                logAction($"Submitted result for test ID [{submitResultRequest?.TestDetails?.Id}]");
            }
            catch (HttpRequestException ex)
            {
                logAction("Warning - there was an error when attempting to connect to the API; please check your configuration.");
            }
        }

        private static HttpClientHandler GetCertIgnoreHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            return handler;
        }
    }
}