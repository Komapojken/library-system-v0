namespace LibrarySystem.Domain.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // list of books
    public List<Book> Books { get; set; } = new();
}