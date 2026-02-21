using LibrarySystem.Data;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Domain;

public class Library
{
    public void LoanBook(int memberId, int bookId)
    {
        using var dbContext = new LibraryDbContext();
        using var transaction =  dbContext.Database.BeginTransaction();
        
        var member = dbContext.Members.Find(memberId);
        var book =  dbContext.Books.Find(bookId);

        if (member == null)
            throw new Exception("Member not found...");

        if (book == null)
            throw new Exception("Book not found...");
        
        if (book.IsOnLoan)
            throw new Exception("Book is already on loan...");
        
        try
        {
            var loan = new Loan
            {
                MemberId = memberId,
                BookId = bookId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30),
            };
        
            book.IsOnLoan = true;
        
            dbContext.Loans.Add(loan);
            dbContext.SaveChanges();
            
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void ReturnBook(int loanId)
    {
        using var dbContext = new LibraryDbContext();
        
        var loan = dbContext.Loans
            .Include(l  => l.Book)
            .FirstOrDefault(l => l.Id == loanId);
        
        if (loan == null)
            throw new Exception("Loan not found...");
        
        loan.EndDate = DateTime.Today;
        loan.Book.IsOnLoan = false;
        
        dbContext.SaveChanges();
    }

    public bool LoanExists(int loanId)
    {
        using var dbContext = new LibraryDbContext();
        return dbContext.Loans
            .AsNoTracking()
            .Any(l => l.Id == loanId);
    }
    
    public void ShowAllBooks()
    {
        using var dbContext = new LibraryDbContext();
        
        var books = dbContext.Books
            .AsNoTracking()
            .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Isbn,
                    b.PublishDate,
                    b.IsOnLoan,
                    Authors = b.Authors.Select(a => a.Name).ToList()
                })
            .ToList();

        if (!books.Any())
            throw new Exception("No books found...");
        
        foreach (var book in books)
        {
            Console.WriteLine($"Id : {book.Id}");
            Console.WriteLine($"Title : {book.Title}");
            Console.WriteLine($"ISBN : {book.Isbn}");
            Console.WriteLine($"Published: {book.PublishDate:yyyy-MM-dd}");
            if (!book.Authors.Any())
            {
                Console.WriteLine("Authors : (none)");
            }
            else
            {
                Console.Write("Authors : ");
                foreach (var author in book.Authors)
                {
                    Console.Write($" - {author}");
                }
                Console.Write("\n");
            }
            Console.WriteLine($"On loan : {book.IsOnLoan}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public void ShowActiveLoans()
    {
        using var dbContext = new LibraryDbContext();

        var loans = dbContext.Loans
            .AsNoTracking()
            .Where(l => l.Book.IsOnLoan)
            .Select(l => new
            {
                l.Id,
                BookTitle = l.Book.Title,
                MemberName = l.Member.Name,
                l.EndDate,
                l.Book.IsOnLoan
            })
            .ToList();

        if (!loans.Any())
            throw new Exception("No active loans found...");
        
        foreach (var loan in loans)
        {
            Console.WriteLine($"Loan ID: {loan.Id}");
            Console.WriteLine($"Book : {loan.BookTitle}");
            Console.WriteLine($"Member : {loan.MemberName}");
            Console.WriteLine($"Due date: {loan.EndDate:yyyy-MM-dd}");
            Console.WriteLine($"On loan : {loan.IsOnLoan}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public int GetActiveLoanCount()
    {
        using var dbContext = new LibraryDbContext();
        
        return dbContext.Loans
            .Count(l => l.Book.IsOnLoan);
    }

    public void ShowAllMembers()
    {
        using var dbContext = new LibraryDbContext();
        
        var members = dbContext.Members
            .AsNoTracking()
            .ToList();

        if (!members.Any())
            throw new Exception("No active members found...");
        
        foreach (var member in members)
        {
            Console.WriteLine($"Member ID: {member.Id}");
            Console.WriteLine($"Member Name: {member.Name}");
            Console.WriteLine($"Member Email: {member.Email}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public void ShowAllAuthors()
    {
        using var dbContext = new LibraryDbContext();

        var authors = dbContext.Authors
            .AsNoTracking()
            .ToList();
        
        if (!authors.Any())
            throw new Exception("No active authors found...");
        
        foreach (var author in authors)
        {
            Console.WriteLine($"Author ID: {author.Id}");
            Console.WriteLine($"Author Name: {author.Name}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public void AddMember(string name, string email, string phoneNumber)
    {
        using var dbContext = new LibraryDbContext();
        
        var exists = dbContext.Members
            .Any(m => m.Email == email);

        if (exists)
            throw new Exception("Member with that e-mail already exists...");

        var member = new Member
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber
        };
        
        dbContext.Members.Add(member);
        dbContext.SaveChanges();
    }

    public void EditMember(Member editedMember)
    {
        using  var dbContext = new LibraryDbContext();

        var member = dbContext.Members.Find(editedMember.Id);
        
        if (member == null)
            throw new Exception("Member not found...");
        
        if (!string.IsNullOrWhiteSpace(editedMember.Name))
            member.Name = editedMember.Name;
        
        if (!string.IsNullOrWhiteSpace(editedMember.Email))
        {
            if (!editedMember.Email.Contains("@"))
                throw new Exception("Invalid e-mail address...");
            
            member.Email = editedMember.Email;
        }
        
        if (!string.IsNullOrWhiteSpace(editedMember.PhoneNumber))
            member.PhoneNumber = editedMember.PhoneNumber;
        
        dbContext.SaveChanges();
    }

    public void RemoveMember(int memberId)
    {
        using var dbContext = new LibraryDbContext();
        
        var member = dbContext.Members.Find(memberId);
        
        if (member == null)
            throw new Exception("Member not found...");
        
        var hasActiveLoan = dbContext.Loans
            .Any(l => l.MemberId == member.Id && l.EndDate > DateTime.Today);
        
        if (hasActiveLoan)
            throw new Exception("Member has active loan...");
        
        dbContext.Members.Remove(member);
        dbContext.SaveChanges();
    }

    public bool MemberExists(int memberId)
    {
        using var dbContext = new LibraryDbContext();
        return dbContext.Members
            .AsNoTracking()
            .Any(m => m.Id == memberId);
    }

    public Member FindMember(int id)
    {
        using var dbContext = new LibraryDbContext();

        var member =  dbContext.Members
            .AsNoTracking()
            .FirstOrDefault(m => m.Id == id);
        
        if (member == null)
            throw new Exception("Member not found...");
        
        return member;
    }
    
    public void AddBook(string title, string isbn, DateTime publishingDate, string authorName)
    {
        using var dbContext = new LibraryDbContext();
        var author = dbContext.Authors
            .FirstOrDefault(a => a.Name == authorName);
        
        if (author == null)
        {
            author = new Author { Name = authorName };
            dbContext.Authors.Add(author);
            dbContext.SaveChanges();
        }
        
        var book = new Book
        {
            Title = title,
            Isbn = isbn,
            PublishDate = publishingDate,
            Authors = new List<Author> { author }
        };
        
        dbContext.Books.Add(book);
        dbContext.SaveChanges();
    }

    public void EditBook(Book workingBook)
    {
        using var dbContext = new LibraryDbContext();

        var book = dbContext.Books
            .Include(b => b.Authors)
            .FirstOrDefault(b => b.Id == workingBook.Id);
        
        if (book == null)
            throw new Exception("Book not found...");
        
        if (!string.IsNullOrWhiteSpace(workingBook.Title))
            book.Title = workingBook.Title;
        
        if (!string.IsNullOrWhiteSpace(workingBook.Isbn))
            book.Isbn = workingBook.Isbn;
        
        book.PublishDate = workingBook.PublishDate;

        book.Authors.Clear();

        foreach (var author in workingBook.Authors)
        {
            var dbAuthor = dbContext.Authors.Find(author.Id);

            if (dbAuthor != null)
            {
                book.Authors.Add(dbAuthor);
            }
        }
        
        dbContext.SaveChanges();
    }
    
    public void RemoveBook(int bookId)
    {
        using var dbContext = new LibraryDbContext();
        
        var book = dbContext.Books
            .Include(b  => b.Authors)
            .FirstOrDefault(b => b.Id  == bookId);
        
        if (book == null)
            throw new Exception("Book not found...");
        
        var isOnLoan = dbContext.Loans
            .Any(l => l.BookId == book.Id && l.EndDate > DateTime.Today);
        
        if (isOnLoan)
            throw new Exception("Book is currently on loan...");
        
        dbContext.Books.Remove(book);
        dbContext.SaveChanges();
    }

    public bool BookExists(int bookId)
    {
        using var dbContext = new LibraryDbContext();
        return dbContext.Books
            .AsNoTracking()
            .Any(b => b.Id == bookId);
    }
    
    public Book FindBook(int id)
    {
        using var dbContext = new LibraryDbContext();

        var book =  dbContext.Books
            .AsNoTracking()
            .Include(b => b.Authors)
            .FirstOrDefault(b => b.Id == id);
        
        if (book == null)
            throw new Exception("Book not found...");
        
        return book;
    }
    
    public void AddAuthor(string name)
    {
        using var dbContext = new LibraryDbContext();
        var authorExist = dbContext.Authors
            .Any(a => a.Name == name);
        
        if (authorExist)
            throw new Exception("Author already exists...");
        
        dbContext.Authors.Add(new Author { Name = name });
        dbContext.SaveChanges();
    }

    public void EditAuthor(int id, string name)
    {
        using var dbContext = new LibraryDbContext();
        
        var author = dbContext.Authors.Find(id);
        
        if (author == null)
            throw new Exception("Author not found...");
        
        if (!string.IsNullOrWhiteSpace(name))
            author.Name = name;
        
        dbContext.SaveChanges();
    }

    public void RemoveAuthor(int authorId)
    {
        using var dbContext = new LibraryDbContext();
        var author = dbContext.Authors.Find(authorId);
        
        if (author == null)
            throw new Exception("Author not found...");
        
        dbContext.Authors.Remove(author);
        dbContext.SaveChanges();
    }

    public bool AuthorExists(int authorId)
    {
        using var dbContext = new LibraryDbContext();
        return dbContext.Authors
            .AsNoTracking()
            .Any(a => a.Id == authorId);
    }
    
    public Author FindAuthor(int id)
    {
        using var dbContext = new LibraryDbContext();

        var author =  dbContext.Authors
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == id);
        
        if (author == null)
            throw new Exception("Author not found...");
        
        return author;
    }

    public void LoadTestData()
    {
        using var dbContext = new LibraryDbContext();

        if (dbContext.Members.Any() || dbContext.Books.Any())
            return; // test data exists

        var author1 = new Author { Name = "Terry Pratchett" };
        var author2 = new Author { Name = "George Orwell" };
        var author3 = new Author { Name = "J.R.R. Tolkien" };
        var author4 = new Author { Name = "Astrid Lindgren" };
        var author5 = new Author { Name = "Stephen King" };
        
        var member1 = new Member
        {
            Name = "Anna Andersson",
            Email = "anna@fejkmail.se",
            PhoneNumber = "0701234567"
        };
        
        var member2 = new Member
        {
            Name = "Björn Berg",
            Email = "björn@fejkmail.se",
            PhoneNumber = "0709876543"
        };
        
        var member3= new Member
        {
            Name = "Erik Johansson",
            Email = "erik@fejkmail.com",
            PhoneNumber = "0729876543"
        };

        var member4 = new Member
        {
            Name = "Maria Karlsson",
            Email = "maria@fejkmail.com",
            PhoneNumber = "0735551122"
        };

        var book1 = new Book
        {
            Title = "1984",
            Isbn = "9780451524935",
            PublishDate = new DateTime(1949, 6, 8),
            IsOnLoan = false,
            Authors = new List<Author> { author2 }
        };

        var book2 = new Book
        {
            Title = "Guards! Guards!",
            Isbn = "9780375121675",
            PublishDate = new DateTime(1989, 1, 1),
            Authors = new List<Author> { author1 }
        };
        
        var book3 = new Book
        {
            Title = "The Hobbit",
            Isbn = "9780261103344",
            PublishDate = new DateTime(1937, 9, 21),
            IsOnLoan = false,
            Authors = new List<Author> { author3 }
        };

        var book4 = new Book
        {
            Title = "Pippi Longstocking",
            Isbn = "9780140309574",
            PublishDate = new DateTime(1945, 11, 26),
            IsOnLoan = false,
            Authors = new List<Author> { author4 }
        };

        var book5 = new Book
        {
            Title = "The Shining",
            Isbn = "9780385121675",
            PublishDate = new DateTime(1977, 1, 28),
            IsOnLoan = false,
            Authors = new List<Author> { author5 }
        };
        
        var loan1 = new Loan
        {
            Member = member1,
            Book = book1,
            StartDate = DateTime.Today.AddDays(-10),
            EndDate = DateTime.Today.AddDays(20)
        };

        var loan2 = new Loan
        {
            Member = member2,
            Book = book2,
            StartDate = DateTime.Today.AddDays(-5),
            EndDate = DateTime.Today.AddDays(25)
        };
        
        // setting IsOnLoan manually for the seed
        book1.IsOnLoan = true;
        book2.IsOnLoan = true;
        
        dbContext.AddRange(author1, author2, author3, author4, author5);
        dbContext.AddRange(member1, member2, member3, member4);
        dbContext.AddRange(book1, book2, book3, book4, book5);
        dbContext.AddRange(loan1, loan2);
        
        dbContext.SaveChanges();
    }
}