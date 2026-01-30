using BookLibrary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;
}