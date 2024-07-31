using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Configure health checks
builder.Services
    //.AddHealthChecks()
    //.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"])
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.MapReverseProxy();
app.Run();

// Configure authentication
//builder.Services
//    .AddAuthentication()
//    .AddJwtBearer("IdentityApiKey", options =>
//    {
//        options.Authority = builder.Configuration["IdentityUrl"];
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidAudiences = builder.Configuration.GetSection("ValidAudiences").Get<string[]>(),
//        };
//    });

// Configure CORS
//builder.Services
//    .AddCors(options =>
//    {
//        options.AddPolicy("CorsPolicy", policy =>
//        {
//            policy
//                .WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>() ?? [])
//                .AllowAnyMethod()
//                .AllowCredentials()
//                .AllowAnyHeader()
//                .SetIsOriginAllowedToAllowWildcardSubdomains();
//        });
//    });
