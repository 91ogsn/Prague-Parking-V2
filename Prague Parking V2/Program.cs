using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using DataAccessLibrary;
using Prague_Parking_V2;


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
        Console.ReadKey();

        //Visa konfigurationsdata i konsolen
        Console.WriteLine("ConfigFil : {0}", config.ToString());
        Console.WriteLine(priceConfig.ToString());
       
        //Skapa ParkingGarage 
        ParkingGarage garage = new ParkingGarage(config);
        //garage.ParkVehicle(new Car("AAA111", config));
        //garage.ParkVehicle(new Mc("BBB111", config));
        //garage.ParkVehicle(new Mc("CCC111", config));
        Console.WriteLine(garage);
                     
                
        
        // garage.ParkVehicle(new Car("ABC123", config));
        // garage.ParkVehicle(new Mc("MCA001", config));
        // garage.ParkVehicle(new Mc("MCB002", config));
        // garage.ParkVehicle(new Car("XYZ789", config));
        // garage.ParkVehicle(new Mc("MCT003", config));
        // //Spara garage data till Json -fil
        // MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage);
        //Console.WriteLine("Garage sparat utan pris");
        //Console.ReadKey();
        
        
        //Ladda garage data från Json -fil
        if (File.Exists(garageFilePath))
        {
            ParkingGarage loadedGarage = new ParkingGarage(config);
            loadedGarage = MinaFiler.LoadFromFile<ParkingGarage>(garageFilePath);
                   
            //Loopa igenom loadedGarage och lägg till fordonen i garage
            for (int i = 0; i < garage.Garage.Count; i++)
            {
                //Kolla om det finns fordon i parkeringsplatsen, clearar och återställer AvailableSize
                if (garage.Garage[i].ParkedVehicles != null)
                {
                    garage.Garage[i].ParkedVehicles.Clear();
                    garage.Garage[i].AvailableSize = garage.Garage[i].SpotSize;
                }

                if (loadedGarage.Garage[i].ParkedVehicles != null)
                {
                    //Lägg till fordonen i garage
                    foreach (var vehicle in loadedGarage.Garage[i].ParkedVehicles)
                    {

                        if (vehicle.VehicleType == VehicleType.Car)
                        {
                            Car car = new Car(vehicle.RegNumber, config)
                            {
                                ArrivalTime = vehicle.ArrivalTime
                            };
                            garage.Garage[i].AddVehicle(car);

                        }
                        else if (vehicle.VehicleType == VehicleType.MC)
                        {
                            Mc mc = new Mc(vehicle.RegNumber, config)
                            {
                                ArrivalTime = vehicle.ArrivalTime
                            };
                            garage.Garage[i].AddVehicle(mc);

                        }

                    }
                }
            }
            
            AnsiConsole.MarkupLine($"[green]Garage data loaded successful[/]");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Could not find garage data file! Creating a default garage");
            MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage); //save default garage to json file
        }
        Console.WriteLine(garage);
        Console.ReadKey();
        
        //kör metod för menyn
        MenuMethods menu = new MenuMethods();
        menu.MainMenu();


        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();

    }
}