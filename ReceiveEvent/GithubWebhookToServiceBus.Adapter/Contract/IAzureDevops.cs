using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract
{
    public interface IAzureDevops
    {
        public VssConnection Login();
        public Task<string> CreateWorkItem(VssConnection connection,string title,string description);
    }
}