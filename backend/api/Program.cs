using AspNetCoreRateLimit;
using NetEscapades.AspNetCore.SecurityHeaders;
using infrastructure;
using service;

var builder = WebApplication.CreateBuilder(args);

// Load the app settings
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

var app = builder.Build();

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
app.UseCors("SpecificOriginsPolicy");
app.MapControllers();
app.Run();