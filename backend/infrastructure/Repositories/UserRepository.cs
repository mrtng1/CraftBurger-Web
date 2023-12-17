using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure.Repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource), "DataSource is null");
    }
    
    public async Task<bool> UserExists(int userId)
    {
        const string sql = "SELECT COUNT(1) FROM users WHERE id = @UserId;";
        using (var conn = _dataSource.OpenConnection())
        {
            int count = await conn.ExecuteScalarAsync<int>(sql, new { UserId = userId });
            return count > 0;
        }
    }

    public async Task<User> GetUserByUsername(string username)
    {
        const string sql = "SELECT * FROM users WHERE username = @Username;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        const string sql = "SELECT * FROM users;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QueryAsync<User>(sql);
        }
    }
    
    public async Task<User> GetUserById(int userId)
    {
        const string sql = "SELECT * FROM users WHERE id = @UserId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
        }
    }

    public async Task CreateUser(User user)
    {
        const string sql = "INSERT INTO users (username, email, passwordhash, passwordsalt) VALUES (@Username, @Email, @PasswordHash, @PasswordSalt);";
        using (var conn = _dataSource.OpenConnection())
        {
            await conn.ExecuteAsync(sql, user);
        }
    }

    public async Task DeleteUser(int id)
    {
        const string sql = "DELETE FROM users WHERE id = @Id;";
        using (var conn = _dataSource.OpenConnection())
        {
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}