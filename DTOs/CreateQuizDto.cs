using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs;

public class CreateQuizDto
{
    [Required(ErrorMessage = "Question is required")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Question must be between 5 and 500 characters")]
    public string Question { get; set; } = string.Empty;

    [Required(ErrorMessage = "Options are required")]
    [MinLength(2, ErrorMessage = "At least 2 options are required")]
    [MaxLength(6, ErrorMessage = "Maximum 6 options allowed")]
    public List<string> Options { get; set; } = new();

    [Required(ErrorMessage = "Correct answer index is required")]
    [Range(0, 5, ErrorMessage = "Correct answer index must be between 0 and 5")]
    public int CorrectAnswer { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Category must be between 2 and 100 characters")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Difficulty is required")]
    [RegularExpression("^(Easy|Medium|Hard)$", ErrorMessage = "Difficulty must be Easy, Medium, or Hard")]
    public string Difficulty { get; set; } = string.Empty;
}

