using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Fries
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Fries name is required.")]
    [StringLength(100, ErrorMessage = "Fries name cannot exceed 100 characters.")]
    public string FriesName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal FriesPrice { get; set; }

    // New property for the image URL
    public string ImageUrl { get; set; }
}