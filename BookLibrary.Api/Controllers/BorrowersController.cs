using BookLibrary.Api.Data;
using BookLibrary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowersController : ControllerBase
{
    private readonly LibraryContext _context;

    public BorrowersController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrower>>> Get()
    {
        return await _context.Borrowers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Borrower>> Get(int id)
    {
        var borrower = await _context.Borrowers.FindAsync(id);
        if (borrower == null) return NotFound();
        return borrower;
    }

    [HttpPost]
    public async Task<ActionResult<Borrower>> Post(Borrower borrower)
    {
        _context.Borrowers.Add(borrower);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = borrower.Id }, borrower);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Borrower borrower)
    {
        if (id != borrower.Id) return BadRequest();
        _context.Entry(borrower).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var borrower = await _context.Borrowers.FindAsync(id);
        if (borrower == null) return NotFound();
        _context.Borrowers.Remove(borrower);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}