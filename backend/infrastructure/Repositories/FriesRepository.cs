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

    public Fries GetFriesById(int id)
    {
        const string sql = "SELECT * FROM fries WHERE id = @id;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Fries>(sql, new { id = id });
        }
    }

    public async Task<Fries> CreateFries(Fries fries)
    {
        const string sql = @"
        INSERT INTO fries (name, price, imageurl) 
        VALUES (@name, @price, @imageUrl) 
        RETURNING *;
        ";

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Fries>(sql, fries);
        }
    }

    public Fries UpdateFries(int id, Fries fries)
    {
        const string sql = @"
        UPDATE fries 
        SET name = @FriesName, price = @FriesPrice, 
            imageurl = @FriesImageUrl 
        WHERE id = @FriesId 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Fries>(sql, new { FriesName = fries.name, FriesPrice = fries.price, FriesImageUrl = fries.imageUrl, FriesId = id });
        }
    }

    public bool DeleteFries(int id)
    {
        const string sql = "DELETE FROM fries WHERE id = @FriesId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { FriesId = id }) > 0;
        }
    }
}