using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Burger
{   
    // Burger ID
    public int id { get; set; }

    // Burger name
    [Required(ErrorMessage = "Burger name is required.")]
    [StringLength(100, ErrorMessage = "Burger name cannot exceed 100 characters.")]
    public string name { get; set; }

    // Burger price
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal price { get; set; }

    // Burger description
    [Required(ErrorMessage = "Burger description is required.")]
    [StringLength(1000, ErrorMessage = "Burger description cannot exceed 1000 characters.")]
    public string description { get; set; }

    // Burger image URL
    public string? imageUrl { get; set; }
}