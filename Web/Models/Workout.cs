using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class Workout
{
    public int Id { get; set; }
    public string Name { get; set; } // e.g., "Януарска Сила"
    public DateTime Date { get; set; }
    
    // Navigation Property
    [ValidateNever]
    public List<WorkoutLog> WorkoutLogs { get; set; } = new();
}
