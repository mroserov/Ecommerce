using Ecommerce.Common.Middleware;
using Ecommerce.Orders.Application.Interfaces;
using Ecommerce.Orders.Application.Services;
using Ecommerce.Orders.Domain.Repositories;
using Ecommerce.Orders.Domain.UnitOfWork;
using Ecommerce.Orders.Infrastructure.Data;
using Ecommerce.Orders.Infrastructure.Repositories;
using Ecommerce.Orders.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
ConfigureLogging(builder);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Ensure the database is created and migrated
EnsureDatabaseMigrated(app);

// Configure middleware
ConfigureMiddleware(app);

app.Run();

void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Host
        .UseSerilog((context, config) =>
        {
            if (!context.HostingEnvironment.IsDevelopment())
            {
                config.WriteTo.Console();
            }
            else
            {
                var elasticsearchUrl = context.Configuration["ELASTICSEARCH_URL"] ?? "http://localhost:9200";
                if (!string.IsNullOrEmpty(elasticsearchUrl))
                {
                    config.Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = "order-api-logs-{0:yyyy.MM.dd}",
                            NumberOfReplicas = 1,
                            NumberOfShards = 2
                        })
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
                }
            }
        });
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Database Configuration
    services.AddDbContext<OrderDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("OrdersDatabase")));

    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IOrderService, OrderService>();

    services.AddAutoMapper(typeof(Program));

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    services.AddControllers();

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
    });

    services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);
}

void EnsureDatabaseMigrated(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<OrderDbContext>();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error($"Error ensure DB {ex.InnerException?.Message ?? ex.Message}");
    }
}
void ConfigureMiddleware(WebApplication app)
{
    var env = app.Environment;

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // Use custom error handler middleware
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // Swagger 
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
    });

    app.UseCors("AllowAllOrigins");

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}
