using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using Serilog;

namespace OutboxPatternDemo.Subscriber
{
    public class Program
    {
        private const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    var duplicateCheckerType = "ttlDistributedCache";

                    switch (duplicateCheckerType)
                    {
                        case "ttlDistributedCache":
                            SetupTtlDistributedCacheDuplicateChecker(services);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                })
                .UseSerilog()
                .UseNServiceBus(ctx =>
                {
                    var endpointConfig = new EndpointConfiguration("OutboxPatternDemo.Subscriber");

                    endpointConfig.UsePersistence<LearningPersistence>();
                    endpointConfig.UseTransport<LearningTransport>();

                    LogManager.Use<SerilogFactory>();

                    var metrics = endpointConfig.EnableMetrics();

                    metrics.SendMetricDataToServiceControl(
                        SERVICE_CONTROL_METRICS_ADDRESS,
                        TimeSpan.FromSeconds(10));

                    return endpointConfig;
                });

        private static void SetupTtlDistributedCacheDuplicateChecker(IServiceCollection services)
        {
            // use memory distributed cache for demo
            services.AddDistributedMemoryCache();
            services.AddTransient<IDuplicateChecker, TtlDistributedCacheDuplicateChecker>(
                ctx => new TtlDistributedCacheDuplicateChecker(ctx.GetService<IDistributedCache>(), TimeSpan.FromMinutes(10)));
        }
    }
}
