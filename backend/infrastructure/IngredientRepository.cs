using api.Models;
using Dapper;
using Npgsql;

namespace infrastructure;

public class IngredientRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public IngredientRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }
    
    public async Task<IEnumerable<Ingredient>> GetIngredientByBurgerId(int burgerId)
    {
        const string sql = @"
                SELECT i.id, i.name 
                FROM ingredients i
                INNER JOIN burgeringredients bi ON i.id = bi.ingredient_id
                WHERE bi.burger_id = @BurgerId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QueryAsync<Ingredient>(sql, new { BurgerId = burgerId });
        }
    }
    
    public async Task<Ingredient> CreateIngredient(Ingredient ingredient)
    {
        const string sql = "INSERT INTO ingredients (name) VALUES (@Name) RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Ingredient>(sql, ingredient);
        }
    }
    
    public async Task<bool> DeleteIngredient(int ingredientId)
    {
        const string deleteBurgerIngredientsSql = "DELETE FROM burgeringredients WHERE ingredient_id = @IngredientId;";
        const string deleteIngredientSql = "DELETE FROM ingredients WHERE id = @IngredientId;";
    
        using (var conn = _dataSource.OpenConnection())
        {
            await conn.ExecuteAsync(deleteBurgerIngredientsSql, new { IngredientId = ingredientId });
            return await conn.ExecuteAsync(deleteIngredientSql, new { IngredientId = ingredientId }) > 0;
        }
    }
    
    public async Task<bool> AddIngredientToBurger(int burgerId, int ingredientId)
    {
        const string sql = "INSERT INTO burgeringredients (burger_id, ingredient_id) VALUES (@BurgerId, @IngredientId);";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.ExecuteAsync(sql, new { BurgerId = burgerId, IngredientId = ingredientId }) > 0;
        }
    }
}