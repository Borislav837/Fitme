using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class WorkoutLog
{
    public int Id { get; set; }
    public int? WorkoutId { get; set; }
    [Required(ErrorMessage = "Моля, изберете упражнение!")]
    public int? ExerciseId { get; set; }
    
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public double? Weight { get; set; }

    // Navigation Properties
    [ValidateNever] 
    public Workout Workout { get; set; }
    [ValidateNever] 
    public Exercise Exercise { get; set; }
}
