using infrastructure.Models;

namespace service.Interfaces;

public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    Task CreateUserAsync(string username, string email, string password);
    bool ValidateToken(string token);
    Task<User> GetUserByIdAsync(int userId);

}