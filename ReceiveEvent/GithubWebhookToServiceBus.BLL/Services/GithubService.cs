using Microsoft.Extensions.Logging;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Contract;


namespace Sharp.Integration.GitHubToDevOpsApp.BLL.Services
{
    public class GithubService:IGithubService
    {
        private readonly IGithub _githubAdapter;
        public GithubService(IGithub githubAdapter)
        {
            _githubAdapter = githubAdapter;
        }

        public async Task<string> FetchCommitDetails(string repositoryOwner, string repositoryName, string commitId)
        {
            Logging<GithubService> logging = new Logging<GithubService>();
            ILogger _logger = logging.loggerObj();
            try
            {
                string commitDetails = await _githubAdapter.GetCommitDetails(repositoryOwner, repositoryName, commitId);
                return commitDetails;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception occured in GithubService:" + ex.Message);
                return ex.Message;
            }
        }
    }
}