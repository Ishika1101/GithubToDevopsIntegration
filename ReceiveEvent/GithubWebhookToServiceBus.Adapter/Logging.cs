using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Integration.GithubToDevopsApp.Adapters
{
    public   class Logging<T>
    {
        public  ILogger loggerObj()
        {
            IServiceCollection services = new ServiceCollection();
            var channel = new ServerTelemetryChannel();

            services.Configure<TelemetryConfiguration>(
                (config) =>
                {
                    config.TelemetryChannel = channel;
                });
            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights("067d8a7f-c277-4389-b95e-975537d7f8ad");
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ILogger<T> logger = serviceProvider.GetService<ILogger<T>>();
            return logger;

        }
    }
}
