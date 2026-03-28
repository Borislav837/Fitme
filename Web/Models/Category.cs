namespace Web.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } // e.g., "Силови"
    
    // Navigation Property
    public List<Exercise> Exercises { get; set; } = new();
}