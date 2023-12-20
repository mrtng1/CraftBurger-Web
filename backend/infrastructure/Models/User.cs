using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

}