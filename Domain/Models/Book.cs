namespace LibrarySystem.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public DateTime PublishDate { get; set; }
    public Boolean IsOnLoan { get; set; }
    
    // list of authors
    public List<Author> Authors { get; set; } = new();
}