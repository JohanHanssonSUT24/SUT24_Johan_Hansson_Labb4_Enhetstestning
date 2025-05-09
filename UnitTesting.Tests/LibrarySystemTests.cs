using SUT24_Johan_Hansson_Labb4_Enhetstestning;

namespace UnitTesting.Tests;

[TestClass]
public class LibrarySystemTests
{
    [TestMethod]
    public void AddingBooks_ShouldFail_IfNoISBNIsAdded()
    {
        var library = new LibrarySystem();
        var book = new Book("Test titel", "Test f�rfattare", "", 1900);

        bool result = library.AddBook(book);

        Assert.IsFalse(result, "AddBook-metoden till�ter tomt ISBN-nr");
    }
    [TestMethod]
    public void AddingBooks_ShouldFail_IfISBNExists()
    {
        var library = new LibrarySystem();
        var book1 = new Book("Test titel", "Test f�rfattare", "1234567890", 2000);
        var book2 = new Book("Test titel", "Test f�rfattare", "1234567890", 2000);

        library.AddBook(book1);
        bool result = library.AddBook(book2);

        Assert.IsFalse(result, "AddBook-metoden till�ter dubbletter av ISBN-nr");
    }
    [TestMethod]
    public void SearchBooksByTitle_ShouldFail_IfCaseSensitive()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("lord of the flies");

        Assert.IsTrue(results.Count > 0, "Sm� bokst�ver rakt igenom");
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
        List<Book> results = library.SearchByTitle("william golding");

        Assert.IsTrue(results.Count > 0, "Sm� bokst�ver rakt igenom");
    }
    [TestMethod]
    public void SearchBooksByAuthor_ShouldFail_IfNotCompleteTitle()
    {
        var library = new LibrarySystem();
        List<Book> results = library.SearchByTitle("Will");

        Assert.IsTrue(results.Count > 0, "Del av f�rfattarens namn r�cker inte");
    }
}
