namespace api.Models;

public class Ingredient
{
    public int ID { get; set; }
    public string Name { get; set; }
    
    public Ingredient() { }
    
    public Ingredient(int id, string name)
    {
        ID = id;
        Name = name;
    }
}