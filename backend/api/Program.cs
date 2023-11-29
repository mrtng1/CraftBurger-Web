using System.Text;
using AspNetCoreRateLimit;
using infrastructure;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using service.Interfaces;
using service.Services;
using service.Interfaces.Blob;
using service.Services.Blob;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Load the app settings
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_Key"));
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["AzureBlobStorageConnectionString"];
    var containerName = configuration["BlobContainerName"];
    return new BlobStorageService(connectionString, containerName);
});

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddSingleton<BurgerRepository>();
builder.Services.AddSingleton<IBurgerService, BurgerService>();

builder.Services.AddSingleton<FriesRepository>();
builder.Services.AddSingleton<IFriesService, FriesService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpecificOriginsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

/* !!!!!turn on before deployment!!!!!!
 
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; object-src 'none';");
    await next();
});
*/

var app = builder.Build();

// First, apply CORS policy
app.UseCors("SpecificOriginsPolicy");

// Then, other middlewares in the correct order
app.UseAuthentication();
app.UseAuthorization();

// Rate limiting
app.UseIpRateLimiting();

// Swagger configuration
app.UseSwagger();
app.UseSwaggerUI();

// Security headers and Content Security Policy
var policyCollection = new HeaderPolicyCollection()
    .AddDefaultSecurityHeaders()
    .AddContentSecurityPolicy(builder =>
    {
        builder.AddDefaultSrc().Self().From("http://localhost:4200");
    });

app.UseSecurityHeaders(policyCollection);

// Map controllers
app.MapControllers();

// Run the application
app.Run();
