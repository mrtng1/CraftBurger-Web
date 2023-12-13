using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Fries
{
    // Fries ID
    [Key]
    public int id { get; set; }

    // Fries name
    [Required(ErrorMessage = "Fries name is required.")]
    [StringLength(30, ErrorMessage = "Fries name cannot exceed 30 characters.")]
    [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Fries name must contain letters and spaces only.")]
    public string name { get; set; }

    // Fries price
    [Required(ErrorMessage = "Price is required.")]
    [Range(10, 100, ErrorMessage = "Price must be greater than 0 and less than or equal to 100.")]
    public decimal price { get; set; }

    // Fries image URL
    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    public string? imageUrl { get; set; }
}