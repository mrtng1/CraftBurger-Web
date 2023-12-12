using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Total price must be greater than 0.")]
    public decimal TotalPrice { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }
    
    public List<OrderDetail> OrderDetails { get; set; }
}