using infrastructure.Models;

namespace service.Interfaces;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task CreateUser(string username, string email, string password);
    bool ValidateToken(string token);
    Task<User> GetUserById(int userId);
    Task<IEnumerable<User>> GetAllUsers();
    Task<bool> DeleteUser(int id);

}