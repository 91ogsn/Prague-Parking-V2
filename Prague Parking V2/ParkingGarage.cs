using DataAccessLibrary;
using Prague_Parking_V2.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Prague_Parking_V2
{
    public class ParkingGarage
    {
        // Properties
        public List<ParkingSpot> Garage { get; set; } 
        public Config Config { get; set; }
        public int Size { get; set; }
        // Constructor
        public ParkingGarage() 
        {
            Garage = new List<ParkingSpot>();
            
        }
        public ParkingGarage(Config config)
        {
            Config = config;
            Size = config.GarageNrOfSpots; //Storleken på garaget hämtas från konfigfil
            Garage = new List<ParkingSpot>(Size);
            for (int i = 1; i <= Size; i++)
            {
                Garage.Add(new ParkingSpot(spotNumber: i));
            }
        }

        // Metoder
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"ParkingGarage Size: {Size}, Total Spots: {Garage.Count}");
            foreach (var spot in Garage)
            {
                info.AppendLine(spot.ToString());
            }
            return info.ToString();
        }

        //Save Garage to file metod
        public static void SaveGarageToFile(ParkingGarage garage)
        {
            string garageFilePath = "../../../garage.json";
            MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage);
            AnsiConsole.MarkupLine($"[green]Garage saved to file successfully![/]");
        }
        //Load Garage from file metod
        public static ParkingGarage LoadGarageFromFile(ParkingGarage garage, Config config)
        {
            string garageFilePath = "../../../garage.json";
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
                return garage;
            }
            else
            {
                Console.WriteLine("Could not find Garage data file! Creating a default garage");
                MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage); //save default garage to json file
                return garage;
            }
        }



        //Visa garage innehåll Spot nr kompakt med färkodning Grön = tom, Röd = full, Gul = delvis full.
        //varje rad ska vara 10 spots
        public void DisplayCompactGarageOverview()
        {
            
            AnsiConsole.MarkupLine("\t([lime]Green: [/]Empty, [yellow]Yellow: [/]Partially full, [red]Red: [/]Full)\n");
            int spotsPerRow = 10;
            for (int i = 0; i < Garage.Count; i++)
            {
                var spot = Garage[i];
                string spotInfo = $"|{spot.SpotNumber:D3}| ";
                if (spot.AvailableSize == spot.SpotSize)
                {
                    // Grön för tom plats
                    AnsiConsole.Markup($"[lime]{spotInfo}[/] ");
                }
                else if (spot.AvailableSize == 0)
                {
                    // Röd för full plats
                    AnsiConsole.Markup($"[red]{spotInfo}[/] ");
                }
                else
                {
                    // Gul för delvis full plats
                    AnsiConsole.Markup($"[yellow]{spotInfo}[/] ");
                }
                // Ny rad efter varje 10 spots
                if ((i + 1) % spotsPerRow == 0)
                {
                    Console.WriteLine();
                }
            }
            
        }
        //Skriv ut parkerade fordon med Spectre Table's
        public void DisplayParkedVehicles()
        {
            Table table = new Table();
            table.AddColumn("SpotNr");
            table.AddColumn("Vehicle Type");
            table.AddColumn("RegNr");
            table.AddColumn("Arrival Time");
            table.Title("[underline cyan1]Parked Vehicles In Garage[/]");
            table.Border = TableBorder.Rounded;
            table.BorderColor(Color.Grey);
            table.ShowRowSeparators();
            foreach (var spot in Garage)
            {
                foreach (var vehicle in spot.ParkedVehicles)
                {
                    table.AddRow(spot.SpotNumber.ToString(), vehicle.VehicleType.ToString(), vehicle.RegNumber, vehicle.ArrivalTime.ToString("g"));
                }
            }
            AnsiConsole.Write(table);
        }



        /*
        * Parkera ett fordon*/
        public void ParkVehicle(Vehicle vehicle)
        {
            
            for (int i = 0; i < Garage.Count; i++)
            {
                if (Garage[i].IsThereRoomForVehicle(vehicle))
                {
                    Garage[i].AddVehicle(vehicle);
                    AnsiConsole.MarkupLine($"[green]Park {vehicle.VehicleType} with RegNr: {vehicle.RegNumber} at SpotNr: {Garage[i].SpotNumber}[/]\nPress any key to return...");
                    Console.ReadKey();
                    return; // Fordonet parkerades framgångsrikt
                }
            }
            AnsiConsole.MarkupLine($"[red]No available spot found for {vehicle.VehicleType} with RegNr: {vehicle.RegNumber}\nGarage is full![/]\nPress any key to return...");
            Console.ReadKey();
            return; // Ingen ledig plats hittades

        }

        /* Hämta ut ett fordon
        * Flytta ett fordon
        * Söka efter fordon (regnummer)
        * Visa hela husets innehåll
        * 
        * Vi behöver även några privata hjälpmetoder:
        * Hitta ledig plats för ett fordon
        * Skapa ett fordon
        * Ta fram ett fordon, givet ett regnummer (variant på sökning)
        */


    }
}