using SUT24_Johan_Hansson_Labb4_Enhetstestning;

namespace UnitTesting.Tests;

[TestClass]
public class LibrarySystemTests
{
    [TestMethod]
    public void AddingBooks_ShouldAddNewBookToDataBase()
    {
        var library = new LibrarySystem();
        var book = new Book("Test titel", "Test författare", "ISBN 0123456789", 1900);

        bool result = library.AddBook(book);

        Assert.IsTrue(result);
        Assert.IsNotNull(library.SearchByISBN("ISBN 0123456789"));
    }
}
