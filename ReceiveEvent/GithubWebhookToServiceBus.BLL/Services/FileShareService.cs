
using Microsoft.Extensions.Logging;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GithubToDevopsApp.Adapters.DataAccessObjects;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Contract;


namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Services
{
    public class FileShareService : IFileShareService
    {
        private readonly IFileShare _fileShareObj;
        public FileShareService(IFileShare fileShareObj) 
        { 
            _fileShareObj = fileShareObj;
        }

        public async Task<string> UploadCodeToFileShare(List<CodeChanges> filesList)
        {
            Logging<FileShareService> logging = new Logging<FileShareService>();
            ILogger _logger = logging.loggerObj();

            try
            {
                return await _fileShareObj.UploadCode(filesList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception occured in FileShareService:" + ex.Message);
                return ex.Message;
            }
        }
    }
}