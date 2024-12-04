using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using OutboxPatternDemo.Bookings.DuplicateCheckers;
using OutboxPatternDemo.Bookings.DuplicateCheckers.CircularBuffer;
using OutboxPatternDemo.Bookings.DuplicateCheckers.DistributedCache;
using OutboxPatternDemo.Bookings.DuplicateCheckers.Sql;
using OutboxPatternDemo.Bookings.DuplicateCheckers.Sql.Data;
using Serilog;
using Serilog.Events;

namespace OutboxPatternDemo.Bookings;

public class Program
{
    private const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

    public static async Task Main(string[] args)
    {
        Console.Title = "Bookings";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configHost) =>
            {
                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    configHost.AddUserSecrets<Program>();
                }
            })
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
                var endpointConfig = new EndpointConfiguration("OutboxPatternDemo.Bookings");
                endpointConfig.UseSerialization<SystemJsonSerializer>();
                endpointConfig.EnableInstallers();

                var useAzureServiceBus = ctx.Configuration.GetValue<bool>("UseAzureServiceBus");
                if (useAzureServiceBus)
                {
                    var transport = endpointConfig.UseTransport<AzureServiceBusTransport>();
                    transport.ConnectionString(ctx.Configuration.GetConnectionString("AzureServiceBus"));
                    transport.TopicName("OutboxPatternDemo");
                    transport.SubscriptionRuleNamingConvention(x => x.FullName.Length <= 50 ? x.FullName : x.FullName[^50..]); // ASB has a 50 char limit
                }
                else
                {
                    var transport = endpointConfig.UseTransport<LearningTransport>();

                    var metrics = endpointConfig.EnableMetrics();
                    metrics.SendMetricDataToServiceControl(
                        SERVICE_CONTROL_METRICS_ADDRESS,
                        TimeSpan.FromSeconds(10));
                }

                var persistence = endpointConfig.UsePersistence<SqlPersistence>();
                persistence.ConnectionBuilder(() => new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True"));
                persistence.SqlDialect<SqlDialect.MsSqlServer>();

                LogManager.Use<SerilogFactory>();

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
