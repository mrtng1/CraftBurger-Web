using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Burger
{   
    // Burger ID
    [Key]
    public int id { get; set; }

    // Burger name
    [Required(ErrorMessage = "Burger name is required.")]
    [StringLength(30, ErrorMessage = "Burger name cannot exceed 30 characters.")]
    [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Burger name must contain letters and spaces only.")]
    public string name { get; set; }

    // Burger price
    [Required(ErrorMessage = "Price is required.")]
    [Range(70, 200, ErrorMessage = "Price must be greater than 0 and less than or equal to 200.")]
    public decimal price { get; set; }

    // Burger description
    [Required(ErrorMessage = "Burger description is required.")]
    [StringLength(500, ErrorMessage = "Burger description cannot exceed 500 characters.")]
    public string description { get; set; }

    // Burger image URL
    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    public string? imageUrl { get; set; }
}