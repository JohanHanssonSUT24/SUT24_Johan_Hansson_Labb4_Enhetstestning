using SUT24_Johan_Hansson_Labb4_Enhetstestning;

namespace UnitTesting.Tests;

[TestClass]

public class LibrarySystemTests
{

    [TestMethod]
    public void AddingBooks_ShouldFail_IfNoISBNIsAdded()//OK
    {
        var library = new LibrarySystem();
        var book = new Book("Test titel", "Test författare", "", 1900);

        bool result = library.AddBook(book);

        Assert.IsFalse(result, "AddBook-metoden tillåter tomt ISBN-nr");
    }
    [TestMethod]
    public void AddingBooks_ShouldFail_IfISBNExists()//OK
    {
        var library = new LibrarySystem();
        var book1 = new Book("Test titel", "Test författare", "1234567890", 2000);
        var book2 = new Book("Test titel", "Test författare", "1234567890", 2000);

        library.AddBook(book1);
        bool result = library.AddBook(book2);

        Assert.IsFalse(result, "AddBook-metoden tillåter dubbletter av ISBN-nr");
    }
    [TestMethod]
    public void BookCouldBeRemoved_IfNotBorrowed()
    {
        var library = new LibrarySystem();
        var book = new Book("Titel", "Författare", "9780773377565", 2025);
        library.AddBook(book);

        bool result = library.RemoveBook(book.ISBN);

        Assert.IsTrue(result, "Boken borde kunna tas bort om den inte är utlånad");

    }
    [TestMethod]
    public void BookCouldBeRemoved_IfBorrowed()//OK
    {
        var library = new LibrarySystem();
        var isbn = "9780743273565";

        library.BorrowBook(isbn);

        bool result = library.RemoveBook(isbn);

        Assert.IsFalse(result, "Boken kan inte tas bort för att den är utlånad");
    }

    [TestMethod]
    public void SearchByPartialISBN_ShouldFail_IfPartialNumber()
    {
        var library = new LibrarySystem();
        var book = new Book("Titel", "Författare", "9780773377565", 2025);
        library.AddBook(book);

        var results = library.SearchByISBN("1234");

        Assert.IsNull(results, "Sökning med del av ISBN ska inte ge resultat då detta är helt unikt nummer.");

    }

    [TestMethod]
    public void SearchBooksByTitle_ShouldFail_IfNotCaseSensitive()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("lord of the flies");

        Assert.IsTrue(results.Count > 0, "Titelsökning borde vara case sensitive.");
    }

    [TestMethod]
    public void SearchBooksByTitle_ShouldFail_IfNotCompleteTitle()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("Lord");

        Assert.IsTrue(results.Count > 0, "Del av titeln räcker inte");
    }

    [TestMethod]
    public void SearchBooksByAuthor_ShouldFail_IfCaseSensitive()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByAuthor("william golding");

        Assert.IsTrue(results.Count > 0, "Små bokstäver rakt igenom");
    }

    [TestMethod]
    public void SearchBooksByAuthor_ShouldFail_IfNotCompleteTitle()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByAuthor("Will");

        Assert.IsTrue(results.Count > 0, "Del av författarens namn räcker inte");
    }
    [TestMethod]
    public void BorrowBook_ShouldFail_IfAllreadyBorrowed()
    {
        var library = new LibrarySystem();
        var isbn = "9780451524935";

        library.BorrowBook(isbn);

        bool secondBorrow = library.BorrowBook(isbn);

        Assert.IsFalse(secondBorrow, "Det ska inte gå att låna en utlånad bok.");
    }
    [TestMethod]
    public void BorroBook_ShouldSetDate()
    {
        var library = new LibrarySystem();
        var isbn = "9780061120084";

        library.BorrowBook(isbn);

        var book = library.SearchByISBN(isbn);

        Assert.IsNotNull(book.BorrowDate, "Utlåningsdatum sätts inte vid utlånning");
    }
    
    [TestMethod]
    public void ReturnBook_ShouldClearBorrowDate()
    {
        var library = new LibrarySystem();
        var isbn = "9780060850524";

        library.BorrowBook(isbn);
        library.ReturnBook(isbn);

        var book = library.SearchByISBN(isbn);

        Assert.IsNull(book.BorrowDate, "Utlåningsdatum borde nollställas vid återlämning.");
    }
    [TestMethod]
    public void ReturnBook_ShouldFail_IfBookWasNotBorrowed()
    {
        var library = new LibrarySystem();
        var isbn = "9780316769488";

        bool result = library.ReturnBook(isbn);

        Assert.IsFalse(result, "Kan ej lämna tillbaka en bok som inte är utlånad.");
    }
    [TestMethod]
    public void OverDueBooks_ShouldDetectOverDue()
    {
        var library = new LibrarySystem();
        var isbn = "9780547928227";
        var book = library.SearchByISBN(isbn);
        book.IsBorrowed = true;
        book.BorrowDate = DateTime.Now.AddDays(-11);

        bool isOverdue = library.IsBookOverdue(isbn, loanPeriodDays: 10);

        Assert.IsTrue(isOverdue, "Försenade böcker rapporteras inte.");

    }
    [TestMethod]
    public void CalculateFees_ShouldReturnCorrectFee_ButItDoesnt()
    {
        var library = new LibrarySystem();
        var isbn = "9780743273565";

        decimal fee = library.CalculateLateFee(isbn, 4);
        decimal expected = 4 * 0.5m;

        Assert.AreEqual(expected, fee, "Felaktig debitering.");
    }
}
