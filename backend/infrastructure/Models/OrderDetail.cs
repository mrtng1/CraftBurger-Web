using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
    
    [Required]
    public string ItemType { get; set; } //'burgers', 'fries', 'dips', 'sides'
}