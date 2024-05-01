using System.ComponentModel.DataAnnotations;

namespace Curriculum.EF.Models;

public class CreateSignUpRequest
{
    [Required]
    public string? FullName { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    [RegularExpression(@"(?=\w{8,25})(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[!@#%&]).*", ErrorMessage = "{0} should have a min of 8 chars in length & contain at least one char (a-z/A-Z/0-9/!@#%&)")]
    public string? Password { get; set; }

    [Required]
    [RegularExpression(@"(?=\w{8,25})(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[!@#%&]).*", ErrorMessage = "{0} should have a min of 8 chars in length & contain at least one char (a-z/A-Z/0-9/!@#%&)")]
    public string? ConfirmPassword { get; set; }

    public bool Terms { get; set; }
}