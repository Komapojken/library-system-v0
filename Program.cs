using LibrarySystem.Domain;
using LibrarySystem.Presentation;

namespace LibrarySystem;

class Program
{
    static void Main(string[] _)
    {
        // domain layer
        var library = new Library();
        
        // presentation layer
        var menu = new ConsoleMenu(library);
        
        menu.Run();
    }
}