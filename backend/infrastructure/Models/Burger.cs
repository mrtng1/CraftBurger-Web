using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Burger
{   
    [Required(ErrorMessage = "Burger ID is required.")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Burger name is required.")]
    [StringLength(100, ErrorMessage = "Burger name cannot exceed 100 characters.")]
    public string BurgerName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal BurgerPrice { get; set; }

    [Required(ErrorMessage = "Burger description is required.")]
    [StringLength(1000, ErrorMessage = "Burger description cannot exceed 1000 characters.")]
    public string BurgerDescription { get; set; }

    // New property for the image URL
    public string ImageUrl { get; set; }
}