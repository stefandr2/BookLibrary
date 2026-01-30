using BookLibrary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(LibraryContext context)
    {
        // Ensure database is created. For production use migrations instead.
        await context.Database.EnsureCreatedAsync();

        // If there are any books, DB has been seeded already.
        if (context.Books.Any()) return;

        var sample = new List<Book>
        {
            new Book { Title = "Clean Code", Author = "Robert C. Martin", Year = 2008, Summary = "Software craftsmanship." },
            new Book { Title = "1984", Author = "George Orwell", Year = 1949, Summary = "Dystopian novel." },
            new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Summary = "A fantasy adventure." }
        };

        context.Books.AddRange(sample);

        // Add sample borrowers
        var borrowers = new List<Borrower>
        {
            new Borrower { Name = "Alice Johnson", Email = "alice@example.com", Mobile = "+15551234567" },
            new Borrower { Name = "Bob Smith", Email = "bob@example.com", Mobile = "+15557654321" }
        };

        context.Borrowers.AddRange(borrowers);

        await context.SaveChangesAsync();

        // Create a sample active rental (Alice rents "1984")
        var book1984 = context.Books.FirstOrDefault(b => b.Title == "1984");
        var alice = context.Borrowers.FirstOrDefault(b => b.Name == "Alice Johnson");
        if (book1984 != null && alice != null)
        {
            book1984.IsAvailable = false;
            var rental = new Rental
            {
                BookId = book1984.Id,
                BorrowerId = alice.Id,
                RentDate = DateTime.UtcNow.AddDays(-3),
                DueDate = DateTime.UtcNow.AddDays(11),
                ReturnDate = null
            };
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();
        }
    }
}

