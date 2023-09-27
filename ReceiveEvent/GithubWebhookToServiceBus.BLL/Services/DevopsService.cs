using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.WebApi;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Contract;


namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Services
{
    public class DevopsService : IDevopsService
    {
        private readonly IAzureDevops _devops;
        public DevopsService(IAzureDevops devops)
        {
            _devops = devops;
        }

        public async Task<string> CreateItem(string title,string description)
        {
            Logging<DevopsService> logging = new Logging<DevopsService>();
            ILogger _logger = logging.loggerObj();

            try
            {
                VssConnection conn = _devops.Login();
               
                string result= await _devops.CreateWorkItem(conn,title,description);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception occured in DevopsService:" + ex.Message);

                return ex.Message;
            }
        }
    }
}