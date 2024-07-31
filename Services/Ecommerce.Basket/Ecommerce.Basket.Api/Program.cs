using Ecommerce.Basket.Application.Services;
using Ecommerce.Basket.Domain.Interfaces;
using Ecommerce.Basket.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
ConfigureLogging(builder);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();


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
                            IndexFormat = "Basket-api-logs-{0:yyyy.MM.dd}",
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
    // Redis Cache Configuration
    services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configurationOptions = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"], true);
        return ConnectionMultiplexer.Connect(configurationOptions);
    });


    builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



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

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
    });
    // Controller Configuration
    services.AddControllers();

    services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);
}

void ConfigureMiddleware(WebApplication app)
{
    var env = app.Environment;

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // Swagger 
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
    });

    app.UseCors("AllowAllOrigins");

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}

