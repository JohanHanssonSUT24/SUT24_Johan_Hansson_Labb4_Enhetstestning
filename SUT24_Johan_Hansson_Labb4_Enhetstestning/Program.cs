namespace SUT24_Johan_Hansson_Labb4_Enhetstestning
{
    public class Program
    {
        static void Main(string[] args)
        {
            LibrarySystem library = new LibrarySystem();
            UserInterface.DisplayMenu(library);
        }
    }
}
