using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Sharp.Integration.GithubToDevopsApp.Adapters;
using Sharp.Integration.GithubToDevopsApp.Adapters.DataAccessObjects;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Adapters;
using Sharp.Integration.GitHubToDevOpsApp.Adapters.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Contract;
using Sharp.Integration.GitHubToDevOpsApp.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GithubToDevopsApp
{
    public class SubscriptionInfo
    {
        public async void GetServiceBusCount(object state)
        {
            string serviceBusConnectionString = "Endpoint=sb://servicebuspayload.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FTXKs6LCemYMIZaKLK5wB8/hZDGDFcKk8+ASbJNsvzg=";
            string topicName = "payloadtopic";
            string subscriptionName = "payloadsubscription";



            var managementClient = new ManagementClient(serviceBusConnectionString);



            try
            {
                // Get the subscription information
                SubscriptionRuntimeInfo subscriptionInfo = await managementClient.GetSubscriptionRuntimeInfoAsync(topicName, subscriptionName);



                if (subscriptionInfo.MessageCount > 0)
                {
                    // Data is available in the subscription
                    Console.WriteLine("Data is available in the subscription.");
                    Console.WriteLine($"Total Message Count: {subscriptionInfo.MessageCount}");
                    IHost host = CreateHost();
                    ServiceBusService dataObj = ActivatorUtilities.CreateInstance<ServiceBusService>(host.Services);
                    string message = await dataObj.ReceiveFromServiceBus();
                    ReceiveCommitData obj = JsonConvert.DeserializeObject<ReceiveCommitData>(message);
                    //Console.WriteLine(message);
                    /*ReceiveCommitData obj = new ReceiveCommitData();
                    obj.CommitId = "88ac6f4207567abccf6cc996973d936b7659a99e";
                    obj.RepoName = "ProductManagement8";
                    obj.RepoOwner = "9595547694";
                    obj.CommiterName = "Vaibhav Chandrabhan Pawar";*/
                    if (obj != null)
                    {
                        GithubService fetchData = ActivatorUtilities.CreateInstance<GithubService>(host.Services);
                        string response = await fetchData.FetchCommitDetails(obj.RepoOwner, obj.RepoName, obj.CommitId);
                        GithubCodeChanges fileChangeData = JsonConvert.DeserializeObject<GithubCodeChanges>(response);

                        if (fileChangeData != null)
                        {
                            FileShareService shareObj = ActivatorUtilities.CreateInstance<FileShareService>(host.Services);
                            List<CodeChanges> list = fileChangeData.filesList.ToList<CodeChanges>();
                            string result = await shareObj.UploadCodeToFileShare(list);

                            if (result != null)
                            {
                                DevopsService devopsObj = ActivatorUtilities.CreateInstance<DevopsService>(host.Services);
                                string description = "Code Review Pending:" + obj.CommitMessage;
                                string res = await devopsObj.CreateItem(obj.CommiterName, description);
                                Console.WriteLine(res);
                            }
                        }
                    }
                }
                else
                {
                    // No data is available in the subscription
                    Console.WriteLine("No Message available in ServiceBus waiting... ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in SubscriptionInfo: {ex.Message}");
            }
            finally
            {
                await managementClient.CloseAsync();
            }
        }

        private static IHost CreateHost() =>
        Host.CreateDefaultBuilder()
      .ConfigureServices((context, services) =>
      {
          services.AddSingleton<IFileShare, FileShareStorage>();
          services.AddSingleton<IFileShareService, FileShareService>();
          services.AddSingleton<IServiceBusReceiver, ServiceBusAdapter>();
          services.AddSingleton<IServiceBusService, ServiceBusService>();
          services.AddSingleton<IAzureDevops, DevopsAdapter>();
          services.AddSingleton<IDevopsService, DevopsService>();
          services.AddSingleton<IGithub, GithubAdapter>();
          services.AddSingleton<IGithubService, GithubService>();
      })
      .Build();
    }
}
