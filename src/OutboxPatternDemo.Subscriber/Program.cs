using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using OutboxPatternDemo.Subscriber.Data;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using Serilog;
using Serilog.Events;

namespace OutboxPatternDemo.Subscriber;

public class Program
{
    private const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((host, services) =>
            {
                services.AddTransient<IDesignTimeDbContextFactory<DuplicateKeyContext>, DuplicateKeyContextFactory>();
                var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True");
                services.AddDbContext<DuplicateKeyContext>(o => o.UseSqlServer(sqlConnection));

                services.AddTransient<ITransactionalDuplicateChecker, SqlDuplicateChecker>();

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
                endpointConfig.EnableInstallers();

                endpointConfig.UseTransport<LearningTransport>();

                // todo optional ASB transport

                var persistence = endpointConfig.UsePersistence<SqlPersistence>();
                persistence.ConnectionBuilder(() => new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True"));
                persistence.SqlDialect<SqlDialect.MsSqlServer>();

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
        => services.AddTransient<IDuplicateChecker, SqlDuplicateChecker>();

    private static void SetupCircularBufferDuplicateChecker(IServiceCollection services)
        => services.AddSingleton<IDuplicateChecker>(ctx => new CircularBufferDuplicateChecker(new ConcurrentCircularBuffer<Guid>(10)));
}
