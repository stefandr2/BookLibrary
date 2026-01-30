using BookLibrary.Api.Data;
using BookLibrary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Controllers;






















































}
n    private bool BookExists(int id) => _context.Books.Any(e => e.Id == id);    }        return NoContent();        await _context.SaveChangesAsync();        _context.Books.Remove(book);        if (book == null) return NotFound();        var book = await _context.Books.FindAsync(id);    {    public async Task<IActionResult> Delete(int id)
n    [HttpDelete("{id}")]    }        return NoContent();        }            return NotFound();        {        catch (DbUpdateConcurrencyException) when (!BookExists(id))        }            await _context.SaveChangesAsync();        {        try        _context.Entry(book).State = EntityState.Modified;        if (id != book.Id) return BadRequest();    {    public async Task<IActionResult> Put(int id, Book book)
n    [HttpPut("{id}")]    }        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);        await _context.SaveChangesAsync();        _context.Books.Add(book);    {    public async Task<ActionResult<Book>> Post(Book book)
n    [HttpPost]    }        return book;        if (book == null) return NotFound();        var book = await _context.Books.FindAsync(id);    {    public async Task<ActionResult<Book>> Get(int id)
n    [HttpGet("{id}")]    }        return await _context.Books.ToListAsync();    {    public async Task<ActionResult<IEnumerable<Book>>> Get()
n    [HttpGet]    }        _context = context;    {
n    public BooksController(LibraryContext context)    private readonly LibraryContext _context;{public class BooksController : ControllerBase[Route("api/[controller]")]n[ApiController]