namespace LibrarySystem.Domain.Models;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    
    // list of loans
    public List<Loan> Loans { get; set; } = new();
}