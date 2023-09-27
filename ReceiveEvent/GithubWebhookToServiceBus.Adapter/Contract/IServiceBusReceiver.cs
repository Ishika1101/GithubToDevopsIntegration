using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract
{
    public interface IServiceBusReceiver
    {
        public Task<string> ReceiveFromPayloadTopic();
    }
}