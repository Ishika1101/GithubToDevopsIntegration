using Azure.Messaging.ServiceBus;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters
{
    public class ServiceBusAdapter : IServiceBusReceiver
    {
        public Guid ReceiverCorrelationId { get; set; }
        public ServiceBusAdapter() {
            ReceiverCorrelationId = Guid.NewGuid();
        }
        public async Task<string> ReceiveFromPayloadTopic()
        {
            //Getting Logging instance
            
            Logging<ServiceBusAdapter> logging = new Logging<ServiceBusAdapter>();
            ILogger _logger=logging.loggerObj();
            _logger.LogInformation("ServiceBusReceiver");

            ServiceBusClient serviceBusClient = new ServiceBusClient("Endpoint=sb://servicebuspayload.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FTXKs6LCemYMIZaKLK5wB8/hZDGDFcKk8+ASbJNsvzg=");

            ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver
                ("payloadtopic","payloadsubscription",
            new ServiceBusReceiverOptions() 
            { 
                ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete 
            });
            //IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver1.ReceiveMessagesAsync();
            ServiceBusReceivedMessage messages = await serviceBusReceiver.ReceiveMessageAsync();
            dynamic result = messages.Body;
            _logger.LogInformation("Flow Acronym: HSIAF, Component Acronym: SBR, Step Number: 5" +
                   "CorrelationId: " + ReceiverCorrelationId);

            if (result != null)
            {
                _logger.LogInformation("ServiceBusReceiver execute successfully");
                return result.ToString();
            }
            else
            {
                _logger.LogInformation("ServiceBus contain no messages to receive");
                return string.Empty;
            }
        }
    }
}