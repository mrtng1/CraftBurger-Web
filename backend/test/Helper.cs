using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Npgsql;
using Dapper;
using infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace test;

public static class Helper
{
    public static readonly NpgsqlDataSource DataSource;
    public static readonly string ClientBaseUrl = "http://localhost:4200/";
    public static readonly string ApiBaseUrl = "http://localhost:5113/api";

    public static readonly string jwtToken;
    
    public static string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("zswy1X/IG5eLuNcaAwdnX1fFTNUxgkOp809AbPcrtMs=");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("IsAdmin", user.Id == 1 ? "true" : "false"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
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
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=1;",
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
}
