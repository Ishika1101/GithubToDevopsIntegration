using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters
{
    public class GithubAdapter : IGithub
    {
        public Guid GitHubCorrelationId { get; set; }

        private readonly HttpClient _httpClient;
        private readonly string _personalAccessToken;
        string KeyVaultUri = "https://integrationappkeyvault.vault.azure.net/";

        public GithubAdapter()
        {
            GitHubCorrelationId = Guid.NewGuid();
            var KeyVaultSecret = new SecretClient(new Uri(KeyVaultUri), new DefaultAzureCredential());
            var _personalAccessToken = KeyVaultSecret.GetSecret("GithubToken");
            //_personalAccessToken = "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";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _personalAccessToken.Value.Value);
        }

        public async Task<string> GetCommitDetails(string repositoryOwner, string repositoryName, string commitId)
        {
            string apiUrl = $"https://api.github.com/repos/{repositoryOwner}/{repositoryName}/commits/{commitId}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            //Application Insight and logging
            Logging<GithubAdapter> logging = new Logging<GithubAdapter>();
            ILogger _logger = logging.loggerObj();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Flow Acronym: HSIAF, Component Acronym: GHA, Step Number: 6" +
                    "CorrelationId: " + GitHubCorrelationId);

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Github Changes received by commit Id:"+commitId);
                return responseBody.ToString();
            }
            else
            {
                _logger.LogError("Request Failed");
                return $"Request failed with status code: {response.StatusCode}";
            }
        }
    }
}