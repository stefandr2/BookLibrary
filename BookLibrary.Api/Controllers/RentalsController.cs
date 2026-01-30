using BookLibrary.Api.Data;
using BookLibrary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly LibraryContext _context;

    public RentalsController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> Get(bool activeOnly = false)
    {
        var query = _context.Rentals.Include(r => r.Book).Include(r => r.Borrower).AsQueryable();
        if (activeOnly) query = query.Where(r => r.ReturnDate == null);
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> Get(int id)
    {
        var rental = await _context.Rentals.Include(r => r.Book).Include(r => r.Borrower).FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) return NotFound();
        return rental;
    }

    [HttpPost]
    public async Task<ActionResult<Rental>> Post(Rental rental)
    {
        var book = await _context.Books.FindAsync(rental.BookId);
        if (book == null) return BadRequest("Book not found.");
        if (!book.IsAvailable) return BadRequest("Book is not available for rent.");

        var borrower = await _context.Borrowers.FindAsync(rental.BorrowerId);
        if (borrower == null) return BadRequest("Borrower not found.");

        rental.RentDate = rental.RentDate == default ? DateTime.UtcNow : rental.RentDate;
        rental.ReturnDate = null;

        book.IsAvailable = false;
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = rental.Id }, rental);
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var rental = await _context.Rentals.Include(r => r.Book).FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) return NotFound();
        if (rental.ReturnDate != null) return BadRequest("Rental already returned.");

        rental.ReturnDate = DateTime.UtcNow;
        if (rental.Book != null)
        {
            rental.Book.IsAvailable = true;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}