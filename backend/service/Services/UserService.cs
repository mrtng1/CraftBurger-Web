using System.IdentityModel.Tokens.Jwt;
using System.Text;
using infrastructure.Models;
using infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using service.Interfaces;

namespace service.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly string _jwtKey;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public UserService(UserRepository userRepository, string jwtKey)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtKey = jwtKey ?? throw new ArgumentNullException(nameof(jwtKey));
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return null;
        }

        return user;
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }
        return true;
    }
    
    public async Task CreateUserAsync(string username, string email, string password)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepository.CreateUserAsync(user);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    
    
    public bool ValidateToken(string SessionToken)
    {
        if (string.IsNullOrEmpty(_jwtKey))
        {
            throw new InvalidOperationException("JWT key is not set.");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtKey);
        try
        {
            tokenHandler.ValidateToken(SessionToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            // Add additional checks if needed (e.g., check if the username exists in your database)

            return true;
        }
        catch
        {
            // Token is not valid
            return false;
        }
    }
}