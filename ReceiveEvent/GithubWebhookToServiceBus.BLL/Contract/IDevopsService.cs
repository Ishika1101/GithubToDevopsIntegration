using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Contract
{
    public interface IDevopsService
    {
        public Task<string> CreateItem(string title,string description);
    }
}