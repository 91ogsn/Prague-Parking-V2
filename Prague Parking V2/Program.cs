using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using DataAccessLibrary;
using Prague_Parking_V2;

using Spectre.Console;
using Prague_Parking_V2.Models;


// 1 Kör metod ladda konfig från fil
// 2 Kör metod för att ladda parkeringsdata från filer
// 3 kör metod för att hämta prislista

public class Program
{
    public static void Main(string[] args)
    {
        // ===== Filsökvägar ===== \\
        string configFilePath = "../../../config.json";
        string priceListFilePath = "../../../pricelist.json";
        string garageFilePath = "../../../garage.json";

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
        
        //Visa konfigurationsdata i konsolen
        Console.WriteLine("ConfigFil : {0}", config.ToString());
        Console.WriteLine(priceConfig.ToString());

        //Skapa ParkingGarage 
        ParkingGarage garage = new ParkingGarage(config);
        // ===== Parkera några fordon för test ===== \\
        //garage.ParkVehicle(new Car("ABC123", config));
        //garage.ParkVehicle(new Mc("MCA001", config));
        //garage.ParkVehicle(new Mc("MCB002", config));
        //garage.ParkVehicle(new Car("XYZ789", config));
        //garage.ParkVehicle(new Mc("MCT003", config));

        //Spara garage data till Json -fil
        //ParkingGarage.SaveGarageToFile(garage);
        //Console.ReadKey();

        //Ladda garage data från Json -fil
        garage = ParkingGarage.LoadGarageFromFile(garage, config);
        
        Console.ReadKey();
                
        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu(garage, config, priceConfig);


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();

    }
}