﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("getAllUsers")]
    [Authorize(Roles = "true")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        if (users.Any())
        {
            return Ok(users);
        }
        return NotFound("No users found.");
    }
    
    [HttpDelete("deleteUser/{id}")]
    [Authorize(Roles = "true")]
    public async Task<IActionResult> DeleteUserById(int id)
    {
        var user = await _userService.GetUserById(id);
        if (user != null)
        {
            await _userService.DeleteUser(id);
            return Ok("User deleted successfully.");
        }
        return NotFound("User not found.");
    }
    
    [HttpGet("getUserById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserById(id);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound("User not found.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _userService.Authenticate(loginDto.Username, loginDto.Password);
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
            await _userService.CreateUser(createDTO.Username, createDTO.Email, createDTO.Password);
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
            new Claim("IsAdmin", user.Id == 1 ? "true" : "false"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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