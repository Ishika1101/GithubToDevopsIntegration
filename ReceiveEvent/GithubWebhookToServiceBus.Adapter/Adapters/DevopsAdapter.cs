using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.Extensibility;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters
{
    public class DevopsAdapter : IAzureDevops
    {
        public Guid DevOpsCorrelationID { get; set; }

        public DevopsAdapter()
        {
            DevOpsCorrelationID = Guid.NewGuid();
        }
        public VssConnection Login()
        {
            // Create Organization on Azure Devops and use that organization URL
            string orgUrl = "https://dev.azure.com/IntegrationOrg123/";

            //Create Personal Access Token from DevOps Portal
            string KeyVaultUri = "https://integrationappkeyvault.vault.azure.net/";
            var KeyVaultSecret = new SecretClient(new Uri(KeyVaultUri), new DefaultAzureCredential());
            var personalAccessToken = KeyVaultSecret.GetSecret("DevopsToken");
            //string personalAccessToken = "njisjc7rkb4n6r3wksfs75vb3qoxokudxr254n6zhpuq72tpdsmq";

            // It will Create a connection to the Azure DevOps
            VssConnection connection = new VssConnection
                (new Uri(orgUrl),
                new VssBasicCredential(string.Empty, personalAccessToken.Value.Value));
            return connection;
        }

        public async Task<string> CreateWorkItem(VssConnection connection,string title, string description)
        {
            //Application Insight and logging
            Logging<ServiceBusAdapter> logging = new Logging<ServiceBusAdapter>();
            ILogger _logger = logging.loggerObj();

            _logger.LogInformation("Flow Acronym: HSIAF, Component Acronym: DVPSADP, Step Number: 7" +
                "CorrelationId: " + DevOpsCorrelationID);

            //Project Name
            string projectName = "AzureDevopsWorkItems";

            // Get a reference to the Work Item Tracking HTTP client
            WorkItemTrackingHttpClient workItemClient = connection.GetClient<WorkItemTrackingHttpClient>();

            // Create a new Work Item
            JsonPatchDocument workItemDocument = new JsonPatchDocument
            {
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = "Workitem:" + title
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path ="/fields/System.Description",
                    Value= description
                }
            };

            WorkItem result = await workItemClient.CreateWorkItemAsync(workItemDocument, projectName, "Task");

            string response = "WorkItem Created on Azure Devops with Id: " + result.Id;
            _logger.LogInformation("Devops adapter:" + response);
            return response;
        }
    }
}