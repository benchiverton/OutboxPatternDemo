using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;
using Serilog.Events;

namespace OutboxPatternDemo.Publisher;

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
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog()
            .UseNServiceBus(ctx =>
            {
                var endpointConfig = new EndpointConfiguration("OutboxPatternDemo.Publisher");
                endpointConfig.EnableInstallers();

                var transport = endpointConfig.UseTransport<LearningTransport>();
                transport.Transactions(TransportTransactionMode.ReceiveOnly);

                // todo optional ASB transport

                var persistence = endpointConfig.UsePersistence<SqlPersistence>();
                persistence.ConnectionBuilder(() => new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True"));
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                endpointConfig.EnableOutbox();

                LogManager.Use<SerilogFactory>();

                var metrics = endpointConfig.EnableMetrics();

                metrics.SendMetricDataToServiceControl(
                    SERVICE_CONTROL_METRICS_ADDRESS,
                    TimeSpan.FromSeconds(10));

                return endpointConfig;
            });
}
