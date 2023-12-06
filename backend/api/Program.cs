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
using service;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var jwtKey = builder.Configuration["JWT_Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT key is not set in configuration.");
}
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT key is not set in configuration.");
}

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>(provider =>
    new UserService(provider.GetRequiredService<UserRepository>(), jwtKey));

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<BurgerRepository>();
builder.Services.AddSingleton<IBurgerService, BurgerService>();
builder.Services.AddSingleton<FriesRepository>();
builder.Services.AddSingleton<IFriesService, FriesService>();
builder.Services.AddSingleton<MailService>();

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["AzureBlobStorageConnectionString"];
    return new BlobStorageService(connectionString);
});

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

var app = builder.Build();

app.UseCors("SpecificOriginsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();
app.UseSwagger();
app.UseSwaggerUI();

var policyCollection = new HeaderPolicyCollection()
    .AddDefaultSecurityHeaders()
    .AddContentSecurityPolicy(builder =>
    {
        builder.AddDefaultSrc().Self().From("http://localhost:4200");
    });
app.UseSecurityHeaders(policyCollection);

app.MapControllers();

app.Run();
