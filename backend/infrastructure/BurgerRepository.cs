using api.Models;
using Dapper;
using Npgsql;

namespace infrastructure;

public class BurgerRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public BurgerRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }

    public IEnumerable<Burger> GetAllBurgers()
    {
        const string sql = "SELECT * FROM burgers;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Burger>(sql);
        }
    }

    public Burger GetBurgerById(int burgerId)
    {
        const string sql = "SELECT * FROM burgers WHERE id = @BurgerId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { BurgerId = burgerId });
        }
    }

    public async Task<Burger> CreateBurger(Burger burger)
    {
        const string sql = "INSERT INTO burgers (burgername, burgerprice, burgerdescription) VALUES (@BurgerName, @BurgerPrice, @BurgerDescription) RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Burger>(sql, burger);
        }
    }


    public Burger UpdateBurger(int burgerId, Burger burger)
    {
        const string sql = "UPDATE burgers SET burgername = @BurgerName, burgerprice = @BurgerPrice, burgerdescription = @BurgerDescription WHERE id = @BurgerId RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { burger.BurgerName, burger.BurgerPrice, burger.BurgerDescription, BurgerId = burgerId });
        }
    }

    public bool DeleteBurger(int burgerId)
    {
        const string sql = "DELETE FROM burgers WHERE id = @BurgerId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { BurgerId = burgerId }) > 0;
        }
    }
}