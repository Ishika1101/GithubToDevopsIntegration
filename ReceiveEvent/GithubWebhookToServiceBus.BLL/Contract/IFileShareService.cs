using Sharp.Integration.GithubToDevopsApp.Adapters.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Contract
{
    public interface IFileShareService
    {
        public Task<string> UploadCodeToFileShare(List<CodeChanges> filesList);
    }
}
