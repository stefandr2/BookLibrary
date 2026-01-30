using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Api.Models;

public class Rental
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int BorrowerId { get; set; }
    public Borrower? Borrower { get; set; }

    public DateTime RentDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    [NotMapped]
    public bool IsActive => ReturnDate == null;
}