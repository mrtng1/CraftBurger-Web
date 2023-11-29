using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure.Repositories;

public class FriesRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public FriesRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource), "DataSource is null");
    }

    public IEnumerable<Fries> GetAllFries()
    {
        const string sql = "SELECT * FROM fries;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Fries>(sql);
        }
    }

    public Fries GetFriesById(int friesId)
    {
        const string sql = "SELECT * FROM fries WHERE id = @FriesId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Fries>(sql, new { FriesId = friesId });
        }
    }

    public async Task<Fries> CreateFries(Fries fries)
    {
        const string sql = @"
        INSERT INTO fries (friesname, friesprice, friesimgurl) 
        VALUES (@FriesName, @FriesPrice, @FriesImageUrl) 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Fries>(sql, fries);
        }
    }

    public Fries UpdateFries(int friesId, Fries fries)
    {
        const string sql = @"
        UPDATE fries 
        SET friesname = @FriesName, friesprice = @FriesPrice, 
            friesimgurl = @FriesImageUrl 
        WHERE id = @FriesId 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Fries>(sql, new { fries.FriesName, fries.FriesPrice, fries.ImageUrl, FriesId = friesId });
        }
    }

    public bool DeleteFries(int friesId)
    {
        const string sql = "DELETE FROM fries WHERE id = @FriesId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { FriesId = friesId }) > 0;
        }
    }
}