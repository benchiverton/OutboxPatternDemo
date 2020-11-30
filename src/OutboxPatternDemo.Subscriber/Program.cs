using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using OutboxPatternDemo.Subscriber.Data;
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
                    var duplicateCheckerType = "CircularBuffer";

                    switch (duplicateCheckerType)
                    {
                        case "DistributedCache":
                            SetupDistributedCacheDuplicateChecker(services);
                            break;
                        case "Sql":
                            SetupSqlDuplicateChecker(services);
                            break;
                        case "CircularBuffer":
                            SetupCircularBufferDuplicateChecker(services);
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

        private static void SetupDistributedCacheDuplicateChecker(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddTransient<IDuplicateChecker, DistributedCacheDuplicateChecker>(
                ctx => new DistributedCacheDuplicateChecker(ctx.GetService<IDistributedCache>(), TimeSpan.FromMinutes(10)));
        }

        private static void SetupSqlDuplicateChecker(IServiceCollection services)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            services.AddDbContext<DuplicateKeyContext>(o => o.UseSqlite(connection));
            services.AddTransient<IDuplicateChecker, SqlDuplicateChecker>();
        }

        private static void SetupCircularBufferDuplicateChecker(IServiceCollection services)
            => services.AddSingleton<IDuplicateChecker>(ctx => new CircularBufferDuplicateChecker(new ConcurrentCurcularBuffer<int>(10)));
    }
}
