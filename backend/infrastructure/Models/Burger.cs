
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Burger
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Burger name is required.")]
        [StringLength(100, ErrorMessage = "Burger name cannot exceed 100 characters.")]
        public string BurgerName { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal BurgerPrice { get; set; }
    }
}