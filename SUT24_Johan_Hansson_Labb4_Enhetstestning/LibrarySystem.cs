using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUT24_Johan_Hansson_Labb4_Enhetstestning
{
    public class LibrarySystem
    {
        private List<Book> books;

        public LibrarySystem()
        {
            books = new List<Book>();
            // Add some initial books
            books.Add(new Book("1984", "George Orwell", "9780451524935", 1949));
            books.Add(new Book("To Kill a Mockingbird", "Harper Lee", "9780061120084", 1960));
            books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
            books.Add(new Book("The Hobbit", "J.R.R. Tolkien", "9780547928227", 1937));
            books.Add(new Book("Pride and Prejudice", "Jane Austen", "9780141439518", 1813));
            books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", "9780316769488", 1951));
            books.Add(new Book("Lord of the Flies", "William Golding", "9780399501487", 1954));
            books.Add(new Book("Brave New World", "Aldous Huxley", "9780060850524", 1932));
        }

        public bool AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.ISBN)) //Added if statements to check if book.ISBN is empty or allready exists.
                return false;
            if (books.Any(b => b.ISBN == book.ISBN))
                return false;

            books.Add(book);
            return true;
        }

        public bool RemoveBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && !book.IsBorrowed)//Added && to include if book is borrowed.
            {
                books.Remove(book);
                return true;
            }
            return false;
        }

        public Book SearchByISBN(string isbn)
        {
            return books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public List<Book> SearchByTitle(string title)
        {
            List<Book> result = new List<Book> ();

            foreach (Book book in books)
            {
                if (book.Title.ToLower().Contains(title.ToLower()))// Compare titles without considering case sensitiviy.
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public List<Book> SearchByAuthor(string author)
        {
            List<Book> result = new List<Book>();

            foreach (Book book in books)
            {
                if (book.Author.ToLower().Contains(author.ToLower()))
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public bool BorrowBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && !book.IsBorrowed)
            {
                book.IsBorrowed = true;
                book.BorrowDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool ReturnBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && book.IsBorrowed)
            {
                book.IsBorrowed = false;
                book.BorrowDate = null;//Added function to clear borrowdate
                return true;
            }
            return false;
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public decimal CalculateLateFee(string isbn, int daysLate)
        {
            if (daysLate <= 0)
                return 0;

            Book book = SearchByISBN(isbn);
            if (book == null)
                return 0;

            decimal feePerDay = 0.5m;
            return daysLate * feePerDay; //Changed + to *
        }

        public bool IsBookOverdue(string isbn, int loanPeriodDays)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && book.IsBorrowed && book.BorrowDate.HasValue)
            {
                TimeSpan borrowedFor = DateTime.Now - book.BorrowDate.Value;
                return borrowedFor.Days > loanPeriodDays;
            }
            return false;
        }
    }
}
