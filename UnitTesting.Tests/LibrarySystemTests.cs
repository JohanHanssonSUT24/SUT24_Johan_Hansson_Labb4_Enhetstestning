using SUT24_Johan_Hansson_Labb4_Enhetstestning;

namespace UnitTesting.Tests;

[TestClass]

public class LibrarySystemTests
{

    [TestMethod]
    public void AddingBooks_ShouldFail_IfNoISBNIsAdded()//OK
    {
        var library = new LibrarySystem();
        var book = new Book("Test titel", "Test f�rfattare", "", 1900);

        bool result = library.AddBook(book);

        Assert.IsFalse(result, "AddBook-metoden till�ter tomt ISBN-nr");
    }
    [TestMethod]
    public void AddingBooks_ShouldFail_IfISBNExists()//OK
    {
        var library = new LibrarySystem();
        var book1 = new Book("Test titel", "Test f�rfattare", "1234567890", 2000);
        var book2 = new Book("Test titel", "Test f�rfattare", "1234567890", 2000);

        library.AddBook(book1);
        bool result = library.AddBook(book2);

        Assert.IsFalse(result, "AddBook-metoden till�ter dubbletter av ISBN-nr");
    }
    [TestMethod]
    public void BookCouldBeRemoved_IfNotBorrowed()
    {
        var library = new LibrarySystem();
        var book = new Book("Titel", "F�rfattare", "9780773377565", 2025);
        library.AddBook(book);

        bool result = library.RemoveBook(book.ISBN);

        Assert.IsTrue(result, "Boken borde kunna tas bort om den inte �r utl�nad");

    }
    [TestMethod]
    public void BookCouldBeRemoved_IfBorrowed()//OK
    {
        var library = new LibrarySystem();
        var isbn = "9780743273565";

        library.BorrowBook(isbn);

        bool result = library.RemoveBook(isbn);

        Assert.IsFalse(result, "Boken kan inte tas bort f�r att den �r utl�nad");
    }

    [TestMethod]
    public void SearchByPartialISBN_ShouldFail_IfPartialNumber()
    {
        var library = new LibrarySystem();
        var book = new Book("Titel", "F�rfattare", "9780773377565", 2025);
        library.AddBook(book);

        var results = library.SearchByISBN("1234");

        Assert.IsNull(results, "S�kning med del av ISBN ska inte ge resultat d� detta �r helt unikt nummer.");

    }

    [TestMethod]
    public void SearchBooksByTitle_ShouldFail_IfNotCaseSensitive()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("lord of the flies");

        Assert.IsTrue(results.Count > 0, "Titels�kning borde vara case sensitive.");
    }

    [TestMethod]
    public void SearchBooksByTitle_ShouldFail_IfNotCompleteTitle()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("Lord");

        Assert.IsTrue(results.Count > 0, "Del av titeln r�cker inte");
    }

    [TestMethod]
    public void SearchBooksByAuthor_ShouldFail_IfCaseSensitive()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByAuthor("william golding");

        Assert.IsTrue(results.Count > 0, "Sm� bokst�ver rakt igenom");
    }

    [TestMethod]
    public void SearchBooksByAuthor_ShouldFail_IfNotCompleteTitle()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByAuthor("Will");

        Assert.IsTrue(results.Count > 0, "Del av f�rfattarens namn r�cker inte");
    }
    [TestMethod]
    public void BorrowBook_ShouldFail_IfAllreadyBorrowed()
    {
        var library = new LibrarySystem();
        var isbn = "9780451524935";

        library.BorrowBook(isbn);

        bool secondBorrow = library.BorrowBook(isbn);

        Assert.IsFalse(secondBorrow, "Det ska inte g� att l�na en utl�nad bok.");
    }
    [TestMethod]
    public void BorroBook_ShouldSetDate()
    {
        var library = new LibrarySystem();
        var isbn = "9780061120084";

        library.BorrowBook(isbn);

        var book = library.SearchByISBN(isbn);

        Assert.IsNotNull(book.BorrowDate, "Utl�ningsdatum s�tts inte vid utl�nning");
    }
    
    [TestMethod]
    public void ReturnBook_ShouldClearBorrowDate()
    {
        var library = new LibrarySystem();
        var isbn = "9780060850524";

        library.BorrowBook(isbn);
        library.ReturnBook(isbn);

        var book = library.SearchByISBN(isbn);

        Assert.IsNull(book.BorrowDate, "Utl�ningsdatum borde nollst�llas vid �terl�mning.");
    }
    [TestMethod]
    public void ReturnBook_ShouldFail_IfBookWasNotBorrowed()
    {
        var library = new LibrarySystem();
        var isbn = "9780316769488";

        bool result = library.ReturnBook(isbn);

        Assert.IsFalse(result, "Kan ej l�mna tillbaka en bok som inte �r utl�nad.");
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

        Assert.IsTrue(isOverdue, "F�rsenade b�cker rapporteras inte.");

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
