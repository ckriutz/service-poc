using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace service_poc
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // First lets inject HttpClient..
                services.AddSingleton<System.Net.Http.HttpClient>();

                // Now we can add both services. They will run at the same time.
                services.AddHostedService<ConsoleLogService>();
                services.AddHostedService<HttpService>();
            })
            .ConfigureLogging((hostContext, configLogging) =>
            {
                // Add general console logging.
                configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                configLogging.AddConsole();
            });
    }
}
