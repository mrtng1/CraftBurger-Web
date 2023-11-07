using infrastructure;
using service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddSingleton<Repository>();
builder.Services.AddSingleton<IService>();
builder.Services.AddSingleton<Service>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors(options =>

{

    options.SetIsOriginAllowed(origin => true)

        .AllowAnyMethod()

        .AllowAnyHeader()

        .AllowCredentials();

});
app.MapControllers();
app.Run();