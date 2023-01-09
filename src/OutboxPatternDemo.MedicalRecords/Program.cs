using System;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using NServiceBus.Transport;
using Serilog;
using Serilog.Events;

namespace OutboxPatternDemo.MedicalRecords;

public class Program
{
    private const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
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
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog()
            .UseNServiceBus(ctx =>
            {
                var endpointConfig = new EndpointConfiguration("OutboxPatternDemo.MedicalRecords");
                endpointConfig.EnableInstallers();

                var useAzureServiceBus = ctx.Configuration.GetValue<bool>("UseAzureServiceBus");
                if (useAzureServiceBus)
                {
                    var transport = endpointConfig.UseTransport<AzureServiceBusTransport>();
                    transport.ConnectionString(ctx.Configuration.GetConnectionString("AzureServiceBus"));
                    transport.TopicName("OutboxPatternDemo");
                    transport.SubscriptionRuleNamingConvention(x => x.FullName.Length <= 50 ? x.FullName : x.FullName[^50..]); // ASB has a 50 char limit
                    transport.Transactions(TransportTransactionMode.ReceiveOnly); // required for outbox
                }
                else
                {
                    var transport = endpointConfig.UseTransport<LearningTransport>();
                    transport.Transactions(TransportTransactionMode.ReceiveOnly); // required for outbox

                    var metrics = endpointConfig.EnableMetrics();
                    metrics.SendMetricDataToServiceControl(
                        SERVICE_CONTROL_METRICS_ADDRESS,
                        TimeSpan.FromSeconds(10));
                }

                var persistence = endpointConfig.UsePersistence<SqlPersistence>();
                persistence.ConnectionBuilder(() => new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True"));
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                endpointConfig.EnableOutbox();

                LogManager.Use<SerilogFactory>();

                return endpointConfig;
            });
}
