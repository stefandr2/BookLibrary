using System.Text.Json.Serialization;

namespace BookLibrary.Api.Models;

public class Borrower
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Mobile { get; set; }

    [JsonIgnore]
    public List<Rental> Rentals { get; set; } = new();
}