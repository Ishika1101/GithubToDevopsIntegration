using Sharp.Integration.GithubToDevopsApp.Adapters.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract
{
    public interface IFileShare
    {
        public Task<string> UploadCode(List<CodeChanges> filesList);
    }
}