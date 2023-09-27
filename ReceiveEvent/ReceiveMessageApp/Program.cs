using System.Threading.Tasks;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Sharp.Integration.GithubToDevopsApp;

SubscriptionInfo subscriptionInfo = new SubscriptionInfo();
Timer timer = new Timer(subscriptionInfo.GetServiceBusCount, null, TimeSpan.Zero, TimeSpan.FromMinutes(3));
Console.ReadKey();