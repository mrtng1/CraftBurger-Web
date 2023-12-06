using System.ComponentModel.DataAnnotations;

namespace apitests;

public class Burger
{   
    public int id { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    public string? imageUrl { get; set; }
}