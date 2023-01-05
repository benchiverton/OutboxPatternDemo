using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OutboxPatternDemo.MedicalRecords.MedicalRecords.Data;
using OutboxPatternDemo.MedicalRecords.MedicalRecords.Services;
using OutboxPatternDemo.MedicalRecords.Outboxes.Custom;

namespace OutboxPatternDemo.MedicalRecords;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        var dbConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True");

        services.AddEntityFrameworkSqlServer().AddDbContext<MedicalRecordContext>(o => o.UseSqlServer(dbConnection), ServiceLifetime.Transient);
        services.AddTransient<IMedicalRecordCommandService, MedicalRecordCommandService>();
        services.AddTransient<IMedicalRecordQueryService, MedicalRecordQueryService>();

        services.AddEntityFrameworkSqlServer().AddDbContext<CustomOutboxContext>(o => o.UseSqlServer(dbConnection), ServiceLifetime.Transient);
        services.AddTransient<IOutboxMessageBus, CustomOutboxMessageBus>();
        services.AddHostedService<CustomOutboxProcessor>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OutboxPatternDemo Publisher", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OutboxPatternDemo Publisher v1");
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
