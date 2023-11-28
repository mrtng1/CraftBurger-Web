using api.Models;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace infrastructure;

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
        const string checkSql = "SELECT COUNT(*) FROM fries WHERE friesname = @FriesName;";
        const string insertSql = "INSERT INTO fries (friesname, friesprice) VALUES (@FriesName, @FriesPrice) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            var exists = await conn.ExecuteScalarAsync<int>(checkSql, new { FriesName = fries.FriesName });
            if (exists > 0)
            {
                throw new InvalidOperationException("A fries item with the same name already exists.");
            }

            return await conn.QuerySingleAsync<Fries>(insertSql, fries);
        }
    }

    public Fries UpdateFries(int friesId, Fries fries)
    {
        const string sql = "UPDATE fries SET friesname = @FriesName, friesprice = @FriesPrice WHERE id = @FriesId RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Fries>(sql, new { fries.FriesName, fries.FriesPrice, FriesId = friesId });
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