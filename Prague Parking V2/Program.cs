using DataAccessLibrary;
using Prague_Parking_V2;
using System.Text.Json;
using Spectre.Console;
using Prague_Parking_V2.Models;
// att göra

// 1 Kör metod ladda konfig från fil
// 2 Kör metod för att ladda parkeringsdata från filer
// 3 kör metod för att hämta prislista

public class Program
{
    private static void Main(string[] args)
    {
        // ===== Filsökvägar ===== \\
        string configFilePath = "../../../config.json";
        string priceListFilePath = "../../../pricelist.json";
        //string garageDataFilePath = "../../../parkinggarage_data.json";

        //Skapa Config och Ladda konfigurationsfil
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
        
        //Skapa PriceConfig och Ladda prislista
        PriceConfigData priceConfig = new PriceConfigData();
        if (File.Exists(priceListFilePath))
        {
            priceConfig = MinaFiler.LoadFromFile<PriceConfigData>(priceListFilePath);
            AnsiConsole.MarkupLine($"[green]Pricelist loaded successful[/]");
        }
        else
        {
            Console.WriteLine("Could not find pricelist file! Creating a default config");
            MinaFiler.SaveToFile<PriceConfigData>(priceListFilePath, priceConfig); //save defaultPricelist to json file 
        }

        
        Console.ReadKey();
        //Visa konfigurationsdata i konsolen
        Console.WriteLine("ConfigFil : {0}", config.ToString());
        Console.WriteLine(priceConfig.ToString());




        Console.ReadKey();







        //kör metod för menyn
        //MenuMethods menu = new MenuMethods();
        //menu.MainMenu();


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();

    }
}