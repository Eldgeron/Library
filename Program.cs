// See https://aka.ms/new-console-template for more information

using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        //variables
        bool transitionKey = false;


        Library library = new Library();

        Book book1 = new Book("Tresure Island", "R.L.Stivenson", new DateTime(2020, 1, 1));
        Book book2 = new Book("Harry Potter", "J. Roaling", new DateTime(2021, 2, 2));

        library.AddBook(book1);
        library.AddBook(book2);

        library.ListBook();

        do
        {
            Console.WriteLine("Select the operation: \n1 -- add a new book to the lirary \n2 -- remove a book from the library \n3 -- list all book from the library \ne -- exit the application");
            string userAnswer = Console.ReadLine();
            switch (userAnswer)
            {
                case "1":
                    bool innerKey = false;
                    do
                    {
                        try
                        {
                            if (library.IsFull())
                            {
                                throw new InvalidOperationException("\nThe library is full.\n");
                            }
                            Console.WriteLine("Enter a title of the book:");
                            string inputTitle = Console.ReadLine();
                            //create exception for empty string
                            if (string.IsNullOrEmpty(inputTitle))
                            {
                                throw new ArgumentException("Title cannot be empty.");
                            }
                            Console.WriteLine("Enter an author of the book:");
                            string inputAuthor = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputAuthor))
                            {
                                inputAuthor = "noname";
                            }

                            Console.WriteLine("Enter the publication date of the book (yyyy-mm-dd):");
                            string inputDate = Console.ReadLine();

                            DateTime publicationDate;
                            if (string.IsNullOrEmpty(inputDate))
                            {
                                publicationDate = DateTime.MinValue;
                            }

                            else if (!DateTime.TryParse(inputDate, out publicationDate))
                            {
                                throw new ArgumentException("Invalid date format.");
                            }

                            Book newBook = new Book(inputTitle, inputAuthor, publicationDate);
                            library.AddBook(newBook);
                            innerKey = true;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Input error: {ex.Message}");
                            innerKey = false;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Library error: {ex.Message}");
                            innerKey = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                            innerKey = false;
                        }
                    }
                    while (!innerKey);
                    break;
                case "2":
                    try
                    {
                        if (library.Empty())
                        {
                            throw new InvalidOperationException("\nThe library is empty\n");
                        }

                        library.ListBook();
                        Console.WriteLine("Select book to delete from the library:");
                        string userInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(userInput))
                        {
                            Console.WriteLine("You need to select a book title:");
                        }
                        else
                        {
                            try
                            {
                                library.RemoveBook(userInput);
                                Console.WriteLine("Book removed successfully.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                    }
                    catch (InvalidOperationException ex)
                    { 
                        Console.WriteLine($"Error: {ex.Message}");

                    }


            break;
                case "3":
                    try 
                    { 
                        if (library.Empty())
                        {
                            throw new InvalidOperationException("\nThe library is empty\n");
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    library.ListBook();
                    break;
                case "e":
                    transitionKey = true;
                    break;
            }
        }
        while (!transitionKey);
    }
}

public class Book
{
    
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Book()
    {
        Title = "noname"; 
        Author = "noname"; 
        ReleaseDate = DateTime.Now;
    }
    public Book(string title)
    {
        Title = title; 
        Author = "noname"; 
        ReleaseDate = DateTime.Now;
    }
    public Book(string title, string author)
    {
        Title = title; 
        Author = author; 
        ReleaseDate = DateTime.Now;
    }
    public Book(string title, string author, DateTime dt)
    {
        Title = title; 
        Author = author; 
        ReleaseDate = dt;
    }
    //clear from hours
    public string ShortReleaseDate => ReleaseDate.ToShortDateString();

    public void Print()
    {
        Console.WriteLine($"Title: {Title}  \nAuthor: {Author} \nRelese date: {ShortReleaseDate}\n");
    }

}

public class Library
{
    private const int MaxBooks = 5;
    private Dictionary<string, Book> books = new Dictionary<string, Book>();

    public bool IsFull()
    {
        return books.Count >= MaxBooks;
    }
    public bool Empty()
    { 
        return books.Count == 0;
    }
    private string GetBook (Book book)
    {
        return $"{book.Title}-{book.Author}";
    }
    public void AddBook (Book book)
    {
        //creating new excrption when user try to add new book to fiiled library
        if (books.Count >= MaxBooks)
        {
            throw new InvalidOperationException("The library can only contain up to 5 books.");
        }
        string key = GetBook (book);
        if (!books.ContainsKey(key))
        {
            books.Add(key, book);
            Console.WriteLine($"Book {book.Title} by {book.Author} added to the library");
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' by {book.Author} already exists.");
        }
    }

    public void RemoveBook(string title)
    {
        if (books.Count == 0)
        {
            throw new InvalidOperationException("The library if empty.");
        }
        Book bookToRemove = books.Values.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (bookToRemove == null)
        {
            throw new ArgumentException("The book with the specified title does not exist.");
        }
        string key = $"{bookToRemove.Title}-{bookToRemove.Author}";
        if (books.ContainsKey(key))
        {
            books.Remove(key);
            Console.WriteLine($"Book {bookToRemove.Title} by {bookToRemove.Author} removed from the library");
        }
        else
        {
            Console.WriteLine($"Book '{bookToRemove.Title}' by {bookToRemove.Author} doesn’t exist.");
        }

    }

    public void ListBook()
    {

        foreach (var book in books.Values)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Release Date: {book.ReleaseDate.ToShortDateString()}");
        }
    }

}




