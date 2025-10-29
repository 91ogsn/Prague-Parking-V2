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

            if (priceConfig == null)
            {
                priceConfig = new PriceConfigData(); //om inläsningen misslyckas, skapa en ny default prislista
                AnsiConsole.MarkupLine($"[red]Failed to load pricelist, created default pricelist[/]");
                MinaFiler.SaveToFile<PriceConfigData>(priceListFilePath, priceConfig); //save defaultPricelist to json file
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Pricelist loaded successful[/]");
            }
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



        //Ladda garage data från Json -fil
        garage = ParkingGarage.LoadGarageFromFile(garage, config);
        //Console.WriteLine(garage.ToString());
        // uppdaterar config med den sparade i objektet ifall den har ändrats i LoadGarageFromFile() metoden så att de stämmer överens
        config = garage.Config;
        //Console.WriteLine(config.ToString());


        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu(garage, config, priceConfig);


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();

    }
}