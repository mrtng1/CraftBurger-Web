using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using service.Interfaces;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
        if (user != null)
        {
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
        return Unauthorized();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateDTO createDTO)
    {
        if (string.IsNullOrWhiteSpace(createDTO.Password))
            return BadRequest(new { message = "Password is required" });

        try
        {
            await _userService.CreateUserAsync(createDTO.Username, createDTO.Email, createDTO.Password);
            return Ok(new { message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            // Add an admin claim if the user is an admin
            new Claim("IsAdmin", user.Id == 1 ? "true" : "false"),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    [HttpPost("validateToken")]
    public IActionResult ValidateToken([FromBody] TokenDTO tokenDto)
    {
        if (string.IsNullOrEmpty(tokenDto.Token))
        {
            return BadRequest("Token is required.");
        }

        if (_userService.ValidateToken(tokenDto.Token))
        {
            return Ok(true); // Token is valid
        }
        else
        {
            return Ok(false); // Token is not valid
        }
    }
}