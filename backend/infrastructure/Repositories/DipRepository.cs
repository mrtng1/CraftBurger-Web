using api.Models;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace infrastructure;

public class DipRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DipRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }

    public IEnumerable<Dip> GetAllDips()
    {
        const string sql = "SELECT * FROM dips;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Dip>(sql);
        }
    }

    public Dip GetDipById(int dipId)
    {
        const string sql = "SELECT * FROM dips WHERE id = @DipId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Dip>(sql, new { DipId = dipId });
        }
    }

    public async Task<Dip> CreateDip(Dip dip)
    {
        const string checkSql = "SELECT COUNT(*) FROM dips WHERE dipname = @DipName;";
        const string insertSql = "INSERT INTO dips (dipname, dipprice) VALUES (@DipName, @DipPrice) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            var exists = await conn.ExecuteScalarAsync<int>(checkSql, new { DipName = dip.DipName });
            if (exists > 0)
            {
                throw new InvalidOperationException("A dip with the same name already exists.");
            }

            return await conn.QuerySingleAsync<Dip>(insertSql, dip);
        }
    }


    public Dip UpdateDip(int dipId, Dip dip)
    {
        const string sql = "UPDATE dips SET dipname = @DipName, dipprice = @DipPrice WHERE id = @DipId RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Dip>(sql, new { dip.DipName, dip.DipPrice, DipId = dipId });
        }
    }

    public bool DeleteDip(int dipId)
    {
        const string sql = "DELETE FROM dips WHERE id = @DipId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { DipId = dipId }) > 0;
        }
    }
}