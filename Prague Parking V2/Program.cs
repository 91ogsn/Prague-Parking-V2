using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using DataAccessLibrary;
using Prague_Parking_V2;

using Spectre.Console;
using Prague_Parking_V2.Models;


// TODO: rensa bort onödiga kommentarer och test kod


public class Program
{
    public static void Main(string[] args)
    {
        // ===== Filsökvägar ===== \\
        string configFilePath = "../../../config.json";

        // === Skapa Config och Ladda konfigurationsfil === \\
        Config config = new Config();
        if (File.Exists(configFilePath))
        {
            config = MinaFiler.LoadFromFile<Config>(configFilePath);
            AnsiConsole.MarkupLine($"[green]Config loaded successful[/]");
        }
        else
        {
            Console.WriteLine("Could not find konfigurationfile! Creating a default config");
            MinaFiler.SaveToFile<Config>(configFilePath, config); //save default to json file 
        }

        // === Skapa PriceConfig och Ladda sparade priceconfig prislista
        PriceConfigData priceConfig = new PriceConfigData();
        priceConfig = PriceConfigData.LoadPriceConfigFromFile(priceConfig);

        // === Skapa ParkingGarage  Ladda garage data från Json -fil === \\
        ParkingGarage garage = new ParkingGarage(config);
        garage = ParkingGarage.LoadGarageFromFile(garage, config);

        config = garage.Config; // uppdaterar config med den sparade i objektet ifall den har ändrats i LoadGarageFromFile() metoden så att de stämmer överens

        //Visa konfigurationsdata i konsolen
        Console.WriteLine("ConfigFil : {0}", config.ToString());
        Console.WriteLine(priceConfig.ToString());
        Console.ReadKey();


        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu(garage, config, priceConfig);


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();

    }
}