using Ecommerce.Catalog.Api.GraphQL.Mutations;
using Ecommerce.Catalog.Api.GraphQL.Queries;
using Ecommerce.Catalog.Api.GraphQL.Types;
using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Application.Services;
using Ecommerce.Catalog.Domain.Interfaces;
using Ecommerce.Catalog.Domain.Services;
using Ecommerce.Catalog.Domain.UnitOfWork;
using Ecommerce.Catalog.Infrastructure.Data;
using Ecommerce.Catalog.Infrastructure.Messaging;
using Ecommerce.Catalog.Infrastructure.Repositories;
using Ecommerce.Catalog.Infrastructure.Services;
using Ecommerce.Catalog.Infrastructure.UnitOfWork;
using Ecommerce.Common.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Text;

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
                            IndexFormat = "catalog-api-logs-{0:yyyy.MM.dd}",
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
    services.AddDbContext<CatalogDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")));

    services.AddScoped<IFileStorageService, FileStorageService>();

    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddSingleton<RabbitMQClientService>();

    services.AddAutoMapper(typeof(Program));

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

    // GraphQL Configuration
    services.AddGraphQLServer()
        .AddQueryType(d => d.Name("Query"))
            .AddTypeExtension<ProductQueries>()
            .AddTypeExtension<CategoryQueries>()
        .AddMutationType(d => d.Name("Mutation"))
            .AddTypeExtension<ProductMutations>()
        .AddType<ProductType>()
        .AddType<CategoryType>()
        //.AddType<UploadType>()
        .AddFiltering()
        .AddProjections()
        .RegisterDbContext<CatalogDbContext>(DbContextKind.Synchronized);

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

    services.AddControllers()
        // Prevent ReferenceLoop 
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
    });

    services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);
}

void EnsureDatabaseMigrated(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<CatalogDbContext>();
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
        });

    app.UseCors("AllowAllOrigins");

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapGraphQL();
}
