using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Ui.Models;

public class Borrower
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Mobile { get; set; }
}
