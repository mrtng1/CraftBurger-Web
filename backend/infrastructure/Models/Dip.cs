using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Dip
    {
        [Required(ErrorMessage = "Dip ID is required.")]
        public int ID { get; set; }
        [Required(ErrorMessage = "Dip name is required.")]
        [StringLength(100, ErrorMessage = "Dip name cannot exceed 100 characters.")]
        public string DipName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal DipPrice { get; set; }
    }
}