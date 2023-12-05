using infrastructure.Models;

namespace service.Interfaces;

public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    Task CreateUserAsync(string username, string password);
}