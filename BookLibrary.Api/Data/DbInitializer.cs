using BookLibrary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(LibraryContext context)
    {
        // Ensure database is created. For production use migrations instead.
        await context.Database.EnsureCreatedAsync();











}    }        await context.SaveChangesAsync();
n        context.Books.AddRange(sample);        };            new Book { Title = "Clean Code", Author = "Robert C. Martin", Year = 2008, Summary = "Software craftsmanship." }            new Book { Title = "1984", Author = "George Orwell", Year = 1949, Summary = "Dystopian novel." },            new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Summary = "A fantasy adventure." },        {
n        var sample = new List<Book>n        if (context.Books.Any()) return; // DB already seeded