using System.ComponentModel.DataAnnotations;

public class Fries
{
    public int id { get; set; }
    
    public string name { get; set; }
    
    public decimal price { get; set; }
    
    public string? imageUrl { get; set; }
}