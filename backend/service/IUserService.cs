using api.Models;

namespace service;

public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    Task CreateUserAsync(string username, string password);
}