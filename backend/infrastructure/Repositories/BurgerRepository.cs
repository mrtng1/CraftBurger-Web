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
        const string sql = "SELECT id, burgername AS name, burgerprice AS price, burgerdescription AS description, burgerimgurl AS imageUrl FROM burgers;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Burger>(sql);
        }
    }

    public Burger GetBurgerById(int burgerId)
    {
        const string sql = "SELECT * FROM burgers WHERE id = @id;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { BurgerId = burgerId });
        }
    }

    public async Task<Burger> CreateBurger(Burger burger)
    {
        const string sql = @"
        INSERT INTO burgers (burgername, burgerprice, burgerdescription, burgerimgurl) 
        VALUES (@name, @price, @description, @imageUrl) 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Burger>(sql, burger);
        }
    }

    public Burger UpdateBurger(int burgerId, Burger burger)
    {
        const string sql = @"
        UPDATE burgers 
        SET burgername = @BurgerName, burgerprice = @BurgerPrice, 
            burgerdescription = @BurgerDescription, burgerimgurl = @BurgerImageUrl 
        WHERE id = @BurgerId 
        RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QuerySingleOrDefault<Burger>(sql, new { BurgerName = burger.name, BurgerPrice = burger.price, BurgerDescription = burger.description, ImageUrl = burger.imageUrl, BurgerId = burgerId });
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