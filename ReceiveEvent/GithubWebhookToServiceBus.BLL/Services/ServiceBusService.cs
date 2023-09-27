using Microsoft.Extensions.Logging;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.TestManagement.WebApi;

namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Services
{
    public class ServiceBusService : IServiceBusService
    {

        

        private readonly IServiceBusReceiver _receiver;
        public ServiceBusService(IServiceBusReceiver serviceBusReceiver)
        {
           _receiver = serviceBusReceiver;
        }

        public async Task<string> ReceiveFromServiceBus()
        {
            Logging<ServiceBusService> logging = new Logging<ServiceBusService>();
            ILogger _logger = logging.loggerObj();
            

            try
            {
                return await _receiver.ReceiveFromPayloadTopic();
            }
            catch (Exception ex)
            {
                
                _logger.LogInformation("exception occured in ServiceBusReceiver:"+ex.Message);
                return ex.Message;
            }
        }
    }
}