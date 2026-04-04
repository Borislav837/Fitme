using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class Exercise
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Моля, въведете име!")]
    public string Name { get; set; } // e.g., "Лежанка"
    public int CategoryId { get; set; }
    
    // Navigation Properties
    [ValidateNever] 
    public Category Category { get; set; }
    public List<WorkoutLog> WorkoutLogs { get; set; } = new();
}