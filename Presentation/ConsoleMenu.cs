using LibrarySystem.Domain;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Presentation;

public class ConsoleMenu
{
    private readonly Library _library;
    
    public ConsoleMenu(Library library)
    {
        _library = library;
    }
    
    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("--- Library Main Menu ---\n");
        Console.WriteLine("1: Loan a book");
        Console.WriteLine("2: Return a book");
        Console.WriteLine("3: Show all books");
        Console.WriteLine("4: Show active loans");
        Console.WriteLine("5: Show all members");
        Console.WriteLine("6: Show all authors");
        Console.WriteLine("7: Add a member");
        Console.WriteLine("8: Edit a member");
        Console.WriteLine("9: Remove a member");
        Console.WriteLine("10: Add a book");
        Console.WriteLine("11: Edit a book");
        Console.WriteLine("12: Remove a book");
        Console.WriteLine("13: Add an author");
        Console.WriteLine("14: Edit  an author");
        Console.WriteLine("15: Remove an author");
        Console.WriteLine("20: Load testdata");
        Console.WriteLine("0: Quit");
        Console.Write("\nYour choice : ");
    }

    public void Run()
    {
        while (true)
        {
            ShowMainMenu();

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                {
                    LoanBook();
                    PressContinue();
                    break;
                }
                case "2":
                {
                    ReturnBook();
                    PressContinue();
                    break;
                }
                case "3":
                {
                    ShowAllBooks();
                    PressContinue();
                    break;
                }
                case "4":
                {
                    ShowActiveLoans();
                    PressContinue();
                    break;
                }
                case "5":
                {
                    ShowAllMembers();
                    PressContinue();
                    break;
                }
                case "6":
                {
                    ShowAllAuthors();
                    PressContinue();
                    break;
                }
                case "7":
                {
                    AddMember();
                    PressContinue();
                    break;
                }
                case "8":
                {
                    EditMember();
                    PressContinue();
                    break;
                }
                case "9":
                {
                    RemoveMember();
                    PressContinue();
                    break;
                }
                case "10":
                {
                    AddBook();
                    PressContinue();
                    break;
                }
                case "11":
                {
                    EditBook();
                    PressContinue();
                    break;
                }
                case "12":
                {
                    RemoveBook();
                    PressContinue();
                    break;
                }
                case "13":
                {
                    AddAuthor();
                    PressContinue();
                    break;
                }
                case "14":
                {
                    EditAuthor();
                    PressContinue();
                    break;
                }
                case "15":
                {
                    RemoveAuthor();
                    PressContinue();
                    break;
                }
                case "20":
                {
                    LoadTestData();
                    PressContinue();
                    break;
                }
                case "0":
                {
                    return;
                }
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
    
    private void PressContinue()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void Header(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Console.WriteLine(new string('-', 30));
    }

    private void LoanBook()
    {
        try
        {
            int bookId;
            int memberId;
            
            Header("--- Loan a book from the library ---");
            _library.ShowAllBooks();

            while (true)
            {
                Console.Write("\nChoose book id : ");
                string? inputId = Console.ReadLine();
                
                if (string.IsNullOrEmpty(inputId))
                {
                    Console.WriteLine("\nPlease enter a valid book id...");
                    continue;
                }

                if (!int.TryParse(inputId, out bookId))
                {
                    Console.WriteLine("\nMust be a number. Try again...");
                    continue;
                }

                if (!_library.BookExists(bookId))
                {
                    Console.WriteLine("\nBook does not exist...");
                    continue;
                }
                
                // valid id and existing book
                break;
            }

            Header("--- Loan a book from the library ---");
            _library.ShowAllMembers();
            
            while (true)
            {
                Console.Write("\nChoose member id : ");
                string? inputId = Console.ReadLine();
                if (string.IsNullOrEmpty(inputId))
                {
                    Console.WriteLine("\nPlease enter a valid member id...");
                    continue;
                }

                if (!int.TryParse(inputId, out memberId))
                {
                    Console.WriteLine("\nMust be a number. Try again...");
                    continue;
                }
                
                if (!_library.MemberExists(memberId))
                {
                    Console.WriteLine("\nMember does not exist...");
                    continue;
                }
                
                // valid id and existing member
                break;
            }
            
            _library.LoanBook(memberId, bookId);
            Console.WriteLine($"\nBook is now on loan to member ID : [{memberId}]...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void ReturnBook()
    {
        try
        {
            int loanId;
            
            Header("--- Return a book to the library ---");
            _library.ShowActiveLoans();
            
            while (true)
            {
                Console.Write("\nChoose loan id : ");
                string? inputId = Console.ReadLine();
                
                if (string.IsNullOrEmpty(inputId))
                {
                    Console.WriteLine("\nPlease enter a valid loan id...");
                    continue;
                }

                if (!int.TryParse(inputId, out loanId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }
                
                if (!_library.LoanExists(loanId))
                {
                    Console.WriteLine("\nLoan does not exist...");
                    continue;
                }
                
                // valid id and existing loan
                break;
            }
            
            _library.ReturnBook(loanId);
            
            Console.WriteLine("\nBook is now returned to the library...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void ShowAllBooks()
    {
        try
        {
            Header("--- Books in the library ---");
            _library.ShowAllBooks();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void ShowActiveLoans()
    {
        try
        {
            Header("--- Active loans in the library database ---");
            _library.ShowActiveLoans();

            int count = _library.GetActiveLoanCount();
            
            Console.WriteLine($"\nTotal active loans : {count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void ShowAllMembers()
    {
        try
        {
            Header("--- Active members in the library database ---");
            _library.ShowAllMembers();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void ShowAllAuthors()
    {
        try
        {
            Header("--- Active authors in the library database ---");
            _library.ShowAllAuthors();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void AddMember()
    {
        try
        {
            Header("--- Add a member to the library database ---");
            
            Console.Write("\nEnter member name: ");
            string? name = Console.ReadLine();
            
            Console.Write("\nEnter member email: ");
            string? email = Console.ReadLine();
            
            Console.Write("\nEnter member phone number: ");
            string? phoneNumber = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber))
            {
                Console.WriteLine("All fields  are required...");
            }
            else
            {
                _library.AddMember(name, email, phoneNumber);
                Console.WriteLine("\nMember is now added to the library database...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void EditMember()
    {
        try
        {
            int memberId;
            
            Header("--- Edit a member in the library database ---");
            _library.ShowAllMembers();

            while (true)
            {
                Console.Write("\nEnter member id to edit : ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }

                if (!int.TryParse(input, out memberId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }

                if (!_library.MemberExists(memberId))
                {
                    Console.WriteLine("\nMember does not exist...");
                    continue;
                }
                
                break;
            }
            
            var workingMember = _library.FindMember(memberId);

            var tempMember = new Member
            {
                Id = workingMember.Id,
                Name = workingMember.Name,
                Email = workingMember.Email,
                PhoneNumber = workingMember.PhoneNumber
            };
            
            while (true)
            {
                Header($"--- Edit member ID [{tempMember.Id}] in the library database ---");
                Console.WriteLine($"1: Name  : {tempMember.Name}");
                Console.WriteLine($"2: Email  : {tempMember.Email}");
                Console.WriteLine($"3: PhoneNumber: {tempMember.PhoneNumber}");
                Console.WriteLine("4: Save and exit");
                Console.WriteLine("5: Discard and exit");
                Console.Write("\nChoice : ");
                
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    {
                        Console.Write("\nEnter new name : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempMember.Name = input;
                        }
                        
                        break;
                    }
                    case "2":
                    {
                        Console.Write("\nEnter new e-mail : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempMember.Email = input;
                        }
                        
                        break;
                    }
                    case "3":
                    {
                        Console.Write("\nEnter new phone number : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempMember.PhoneNumber = input;
                        }
                        
                        break;
                    }
                    case "4":
                    {
                        try
                        {
                            _library.EditMember(tempMember);
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError: {ex.Message}");
                            PressContinue();
                            break;
                        }
                    }
                    case "5":
                    {
                        Console.WriteLine("\nExiting without saving...");
                        return;
                    }
                    default:
                    {
                        Console.WriteLine("\nInvalid input...");
                        PressContinue();
                        break;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void RemoveMember()
    {
        try
        {
            int memberId;
            
            Header("--- Remove a member from the library database ---");
            _library.ShowAllMembers();

            while (true)
            {
                Console.Write("\nEnter member id to remove : ");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }

                if (!int.TryParse(input, out memberId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }

                if (!_library.MemberExists(memberId))
                {
                    Console.WriteLine("\nMember does not exist...");
                    continue;
                }
                
                break;
            }
            
            _library.RemoveMember(memberId);
            Console.WriteLine("\nMember is now removed from the library database...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void AddBook()
    {
        try
        {
            Header("--- Add a book to the library database ---");
            
            Console.Write("\nEnter title : ");
            string? inputTitle = Console.ReadLine();
            
            Console.Write("\nEnter ISBN : ");
            string? inputIsbn = Console.ReadLine();
            
            Console.Write("\nEnter publish date (yyyy-mm-dd) : ");
            string? inputPublishDate = Console.ReadLine();
            
            Console.Write("\nEnter author name : ");
            string? inputAuthor = Console.ReadLine();

            if (string.IsNullOrEmpty(inputTitle) ||
                string.IsNullOrEmpty(inputIsbn) ||
                string.IsNullOrEmpty(inputPublishDate) ||
                string.IsNullOrEmpty(inputAuthor))
            {
                throw new Exception("All fields are required...");
            }

            if (!DateTime.TryParse(inputPublishDate, out DateTime publishDate))
            {
                throw new Exception("Invalid date format...");
            }
            
            _library.AddBook(inputTitle, inputIsbn, publishDate, inputAuthor);
            Console.WriteLine("\nBook is now added to the library database...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void EditBook()
    {
        try
        {
            int bookId;
            
            while (true)
            {
                Header("--- Edit a book in the library database ---");
                _library.ShowAllBooks();
                
                Console.Write("\nEnter book id to edit : ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\nInvalid input...");
                    PressContinue();
                    continue;
                }

                if (!int.TryParse(input, out bookId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    PressContinue();
                    continue;
                }

                if (!_library.BookExists(bookId))
                {
                    Console.WriteLine("\nBook does not exist...");
                    PressContinue();
                    continue;
                }
                break;
            }
            
            var workingBook = _library.FindBook(bookId);

            var tempBook = new Book
            {
                Id = workingBook.Id,
                Title = workingBook.Title,
                Isbn = workingBook.Isbn,
                PublishDate = workingBook.PublishDate,
                Authors = workingBook.Authors.ToList()
            };
            
            var editBookLoop = true;
            
            while (editBookLoop)
            {
                Header($"--- Edit book id [{tempBook.Id}] in the library database ---");
                Console.WriteLine($"1: Title  : {tempBook.Title}");
                Console.WriteLine($"2: ISBN  : {tempBook.Isbn}");
                Console.WriteLine($"3: Publishing date: {tempBook.PublishDate}");
                if (!tempBook.Authors.Any())
                {
                    Console.WriteLine("4: Authors : (none)");
                }
                else
                {
                    Console.Write("4: Authors : ");
                    foreach (var author in tempBook.Authors)
                    {
                        Console.Write($" - {author.Name}");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("5: Save and exit");
                Console.WriteLine("6: Discard and exit");
                Console.Write("\nChoice : ");
                
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // edit title
                    {
                        Console.Write("\nEnter new title : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempBook.Title = input;
                        }
                        
                        break;
                    }
                    case "2": // edit isbn
                    {
                        Console.Write("\nEnter new ISBN : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempBook.Isbn = input;
                        }
                        
                        break;
                    }
                    case "3": // edit publish date
                    {
                        Console.Write("\nEnter new date (yyyy-mm-dd) : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            if (DateTime.TryParse(input, out DateTime date))
                            {
                                tempBook.PublishDate = date;
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid date...");
                            }
                        }
                        
                        break;
                    }
                    case "4": // edit author
                    {
                        tempBook.Authors = EditBookAuthors(tempBook.Authors);
                        break;
                    }
                    case "5": // save and exit
                    {
                        workingBook = tempBook;
                        _library.EditBook(workingBook);
                        editBookLoop = false;
                        Console.WriteLine("\nSaving changes to the book...");
                        break;
                    }
                    case "6": // discard and exit
                    {
                        Console.WriteLine("\nExiting without saving...");
                        editBookLoop = false;
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("\nInvalid input...");
                        PressContinue();
                        break;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private List<Author> EditBookAuthors(List<Author> authors)
    {
        var newAuthors = authors.ToList();
        
        try
        {
            while (true)
            {
                Header("--- List of authors to edit ---\n");
                Console.WriteLine("1: Add author to book");
                Console.WriteLine("2: Remove author from book");
                Console.WriteLine("3: Save and exit");
                Console.WriteLine("4: Discard and exit\n");
                Console.WriteLine(new string('-', 30));
                
                foreach (var author in newAuthors)
                {
                    Console.WriteLine($"Id: {author.Id}, Name: {author.Name}");
                }
                
                Console.Write("\nChoice : ");
                var choiceEdit =  Console.ReadLine();

                switch (choiceEdit)
                {
                    case "1": // add author to book
                    {
                        
                            
                        while (true)
                        {
                            Header("--- List of authors to add ---");
                            _library.ShowAllAuthors();
                                
                            Console.Write("\nEnter author id to add author to book :");
                            var input = Console.ReadLine();

                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("\nInvalid input...");
                                PressContinue();
                                continue;
                            }

                            if (!int.TryParse(input, out var newAuthorId))
                            {
                                Console.WriteLine("\nInput must be a number. Try again...");
                                PressContinue();
                                continue;
                            }

                            if (!_library.AuthorExists(newAuthorId))
                            {
                                Console.WriteLine("\nAuthor does not exist...");
                                PressContinue();
                                continue;
                            }

                            var insertNewAuthor = _library.FindAuthor(newAuthorId);
                            newAuthors.Add(insertNewAuthor);
                            break;
                        }
                        
                        break;
                    }
                    case "2": // remove author from book
                    {
                        while (true)
                        {
                            Console.Write("\nEnter author id to remove from the book : ");
                            var input = Console.ReadLine();

                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("\nInvalid input...");
                                continue;
                            }

                            if (!int.TryParse(input, out var checkId))
                            {
                                Console.WriteLine("\nInput must be number. Try again...");
                                continue;
                            }

                            bool exist = newAuthors.Any(a => a.Id == checkId);
                                
                            if (!exist)
                            {
                                Console.WriteLine("\nAuthor with that id is not assigned to this book...");
                                continue;
                            }
                                
                            newAuthors.RemoveAll(a => a.Id == checkId);
                            break;
                        }
                        break;
                    }
                    case "3": // save and exit
                    {
                        return newAuthors;
                    }
                    case "4": // discard and exit
                    {
                        return authors;
                    }
                    default:
                    {
                        Console.WriteLine("\nInvalid input...");
                        PressContinue();
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        return authors;
    }

    private void RemoveBook()
    {
        try
        {
            int bookId;
            
            Header("--- Remove a book from the library database ---");
            _library.ShowAllBooks();

            while (true)
            {
                Console.Write("\nEnter book id to remove book : ");
                string? inputId = Console.ReadLine();

                if (string.IsNullOrEmpty(inputId))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }

                if (!int.TryParse(inputId, out bookId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }

                if (!_library.BookExists(bookId))
                {
                    Console.WriteLine("\nBook does not exist...");
                    continue;
                }
                
                break;
            }
            
            _library.RemoveBook(bookId);
            Console.WriteLine("\nBook is removed from library database...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }
    
    private void AddAuthor()
    {
        try
        {
            string? inputName;
            
            Header("--- Add a author to library database ---");

            while (true)
            {
                Console.Write("\nEnter new author name : ");
                inputName = Console.ReadLine();

                if (string.IsNullOrEmpty(inputName))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }
                
                break;
            }

            _library.AddAuthor(inputName);
            Console.WriteLine("\nAuthor is now added to the library database...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }
    
    private void EditAuthor()
    {
        try
        {
            int authorId;
            
            Header("--- Edit an author in the library database ---");
            _library.ShowAllAuthors();

            while (true)
            {
                Console.Write("\nEnter author id to edit : ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }

                if (!int.TryParse(input, out authorId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }

                if (!_library.AuthorExists(authorId))
                {
                    Console.WriteLine("\nAuthor does not exist...");
                    continue;
                }
                
                break;
            }
            
            var workingAuthor = _library.FindAuthor(authorId);
            
            var tempId = workingAuthor.Id;
            var tempName = workingAuthor.Name;
            
            while (true)
            {
                Header($"--- Edit author ID [{tempId}] in the library database ---");
                Console.WriteLine($"1: Name  : {tempName}");
                Console.WriteLine("2: Save and exit");
                Console.WriteLine("3: Discard and exit");
                Console.Write("\nChoice : ");
                
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    {
                        Console.Write("\nEnter new name : ");
                        var input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("\nInvalid input...");
                            PressContinue();
                        }
                        else
                        {
                            tempName = input;
                        }
                        
                        break;
                    }
                    case "2":
                    {
                        try
                        {
                            _library.EditAuthor(tempId, tempName);
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError: {ex.Message}");
                            PressContinue();
                            break;
                        }
                    }
                    case "3":
                    {
                        Console.WriteLine("\nExiting without saving...");
                        return;
                    }
                    default:
                    {
                        Console.WriteLine("\nInvalid input...");
                        PressContinue();
                        break;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private void RemoveAuthor()
    {
        try
        {
            int authorId;
            
            Header("--- Remove a author from the library database ---");
            _library.ShowAllAuthors();

            while (true)
            {
                Console.Write("\nEnter author id to remove from database : ");
                string? inputId = Console.ReadLine();

                if (string.IsNullOrEmpty(inputId))
                {
                    Console.WriteLine("\nInvalid input...");
                    continue;
                }

                if (!int.TryParse(inputId, out authorId))
                {
                    Console.WriteLine("\nInput must be a number. Try again...");
                    continue;
                }

                if (!_library.AuthorExists(authorId))
                {
                    Console.WriteLine("\nAuthor does not exist...");
                    continue;
                }
                
                break;
            }
            
            _library.RemoveAuthor(authorId);
            Console.WriteLine("\nAuthor is now removed from the library database...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    public void LoadTestData()
    {
        _library.LoadTestData();
        Console.WriteLine("\nTestdata loaded...");
    }
}