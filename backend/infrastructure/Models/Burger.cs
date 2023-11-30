using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Burger
{   
    public int ID { get; set; }
    
    public string BurgerName { get; set; }
    
    public decimal BurgerPrice { get; set; }
    
    public string BurgerDescription { get; set; }

    // New property for the image URL
    public string ImageUrl { get; set; }
}