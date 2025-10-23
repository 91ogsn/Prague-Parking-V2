


using Prague_Parking_V2;
// att göra
// 0 lägg till Spectre.console
// 1 Kör metod ladda konfig från fil
// 2 Kör metod för att ladda parkeringsdata från filer
// 3 kör metod för att hämta prislista

internal class Program
{
    private static void Main(string[] args)
    {
        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu();


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }
}