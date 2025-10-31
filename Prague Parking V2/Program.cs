using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using DataAccessLibrary;
using Prague_Parking_V2;

using Spectre.Console;
using Prague_Parking_V2.Models;



public class Program
{
    public static void Main(string[] args)
    {
        // === Skapa Config och Ladda konfigurationsfil === \\
        Config config = new Config();
        config = Config.LoadConfig(config);


        // === Skapa PriceConfig och Ladda sparade prislista === \\
        PriceConfigData priceConfig = new PriceConfigData();
        priceConfig = PriceConfigData.LoadPriceConfigFromFile(priceConfig);

        // === Skapa ParkingGarage och Ladda garage data  === \\
        ParkingGarage garage = new ParkingGarage(config);
        garage = ParkingGarage.LoadGarageFromFile(garage, config);
        config = garage.Config; // uppdaterar config med den sparade i objektet ifall den har ändrats i LoadGarageFromFile() metoden så att de stämmer överens

        // === Visa konfigurationsdata i konsolen === \\
        Console.WriteLine("\nConfigFil : {0}", config.ToString());
        Console.WriteLine(priceConfig.ToString());
        AnsiConsole.MarkupLine("[grey]Press any key to continue to the main menu...[/]");
        Console.ReadKey();

        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu(garage, config, priceConfig);

        //Console.WriteLine("\n\nPress any key to exit...");
        //Console.ReadKey();

    }
}