using System.Text.Json.Serialization;

namespace BookLibrary.Api.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? Summary { get; set; }

    // True when the book is available for rent
    public bool IsAvailable { get; set; } = true;

    [JsonIgnore]
    public List<Rental> Rentals { get; set; } = new();
}