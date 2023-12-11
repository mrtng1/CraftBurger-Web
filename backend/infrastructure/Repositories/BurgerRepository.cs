using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure.Repositories;

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

    public Burger GetBurgerById(int id)
    {
        const string sql = "SELECT * FROM burgers WHERE id = @id;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { id = id });
        }
    }

    public async Task<Burger> CreateBurger(Burger burger)
    {
        const string sql = @"
        INSERT INTO burgers (name, price, description, imageurl) 
        VALUES (@name, @price, @description, @imageUrl) 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Burger>(sql, burger);
        }
    }

    public Burger UpdateBurger(int id, Burger burger)
    {
        const string sql = @"
        UPDATE burgers 
        SET name = @BurgerName, price = @BurgerPrice, 
                   description = @BurgerDescription, imageurl = @BurgerImageUrl 
        WHERE id = @BurgerId 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { BurgerName = burger.name, BurgerPrice = burger.price, BurgerDescription = burger.description, BurgerImageUrl = burger.imageUrl, BurgerId = id });
        }
    }

    public bool DeleteBurger(int id)
    {
        const string sql = "DELETE FROM burgers WHERE id = @id;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { id = id }) > 0;
        }
    }
}