namespace BookLibrary.Ui.Models;

public class CreateRentalRequest
{
    public int BookId { get; set; }
    public int BorrowerId { get; set; }
    public DateTime? DueDate { get; set; }
}
