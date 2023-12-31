using System.Net.Http.Json;
using Npgsql;
using Dapper;

namespace test;

public static class Helper
{
    public static readonly NpgsqlDataSource DataSource;
    public static readonly string ClientBaseUrl = "http://localhost:4200/";
    public static readonly string ApiBaseUrl = "http://localhost:5113/api";

    static Helper()
    {
        var envVarKeyName = "pgconn";
        var rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName);
        if (rawConnectionString == null)
        {
            throw new Exception("Connection string environment variable 'pgconn' is empty.");
        }

        try
        {
            var uri = new Uri(rawConnectionString);
            var properlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=6;",
                uri.Host,
                uri.AbsolutePath.Trim('/'),
                uri.UserInfo.Split(':')[0],
                uri.UserInfo.Split(':')[1],
                uri.Port > 0 ? uri.Port : 5432);
            DataSource = new NpgsqlDataSourceBuilder(properlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception("Connection string is found but could not be used.", e);
        }
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception("There was an error rebuilding the database.", e);
            }
        }
    }

    public static string RebuildScript = @"
DROP TABLE IF EXISTS burgers CASCADE;
CREATE TABLE burgers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(30) NOT NULL,
    price DECIMAL NOT NULL,
    description VARCHAR(500) NOT NULL,
    imageurl VARCHAR(2083) NULL
);

DROP TABLE IF EXISTS fries CASCADE;
CREATE TABLE fries (
    id SERIAL PRIMARY KEY,
    name VARCHAR(30) NOT NULL,
    price DECIMAL NOT NULL,
    imageurl VARCHAR(2083) NULL
);";
    
    public static async Task<string> GetAuthenticationToken(string username, string password)
    {
        using var httpClient = new HttpClient();

        var loginDto = new
        {
            Username = username,
            Password = password
        };

        var response = await httpClient.PostAsJsonAsync("http://localhost:5113/Auth/login", loginDto);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Authentication failed");
        }

        var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return data["token"]; 
    }
}
