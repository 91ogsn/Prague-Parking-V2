using DataAccessLibrary;
using Prague_Parking_V2.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Prague_Parking_V2;

public class ParkingGarage
{
    // Properties
    public List<ParkingSpot> Garage { get; set; }
    public Config? Config { get; set; }
    public int Size { get; set; }
    // Constructor
    public ParkingGarage()
    {
        Garage = new List<ParkingSpot>();

    }
    public ParkingGarage(Config config)
    {
        Config = config;
        Size = config.GarageNrOfSpots; //Storleken på garaget hämtas från konfig
        Garage = new List<ParkingSpot>(Size);
        for (int i = 1; i <= Size; i++)
        {
            Garage.Add(new ParkingSpot(spotNumber: i));
        }
    }

    // ===== Metoder ===== \\
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
    // === Räkna antalet upptagna platser i garaget === \\
    public int CountOccupiedSpots()
    {
        int occupiedSpots = 0;
        foreach (var spot in Garage)
        {
            if (spot.ParkedVehicles.Count > 0)
            {
                occupiedSpots++;
            }
        }
        return occupiedSpots;
    }

    // === Save Garage to file metod === \\
    public static void SaveGarageToFile(ParkingGarage garage)
    {
        string garageFilePath = "../../../garage.json";
        MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage);
        AnsiConsole.MarkupLine($"[green]Garage saved to file successfully![/]");
    }

    // === Load Garage from file metod === \\
    public static ParkingGarage LoadGarageFromFile(ParkingGarage garage, Config config)
    {
        string garageFilePath = "../../../garage.json";
        if (File.Exists(garageFilePath))
        {
            ParkingGarage loadedGarage = new ParkingGarage(config);
            loadedGarage = MinaFiler.LoadFromFile<ParkingGarage>(garageFilePath);
            int ocupiedSpots = loadedGarage.CountOccupiedSpots();

            //om config.garageNrOfSpots är midre än loadedGarage.CountOccupiedSpots så
            //använd den gamla konfigurationen som är sparad i garage filen
            if (config.GarageNrOfSpots < ocupiedSpots)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[red]Loaded garage data has more occupied spots ({ocupiedSpots}) than configured garage size ({config.GarageNrOfSpots}).[/]");
                AnsiConsole.MarkupLine($"[red]Please update your configuration file or clear the garage data file.[/]");
                AnsiConsole.MarkupLine($"[red]Loading saved Garage with old config instead.[/]\n[grey]Press any key to continue..[/]");
                Console.ReadKey();

                //returnerar loadded garage med gammal config
                return loadedGarage;
            }
            else
            {
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
        }
        else
        {
            Console.WriteLine("Could not find Garage data file! Creating a default garage");
            MinaFiler.SaveToFile<ParkingGarage>(garageFilePath, garage); //save default garage to json file
            return garage;
        }
    }

    // === Visa garage innehåll Spot nr kompakt med färkodning === \\

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
    // === Skriv ut parkerade fordon till anv (med Spectre Table's) === \\
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

    // === Parkera ett fordon (första lediga plats) === \\
    public void ParkVehicle(Vehicle vehicle)
    {

        for (int i = 0; i < Garage.Count; i++)
        {
            if (Garage[i].IsThereRoomForVehicle(vehicle))
            {
                Garage[i].AddVehicle(vehicle);
                AnsiConsole.MarkupLine($"[green]Park {vehicle.VehicleType} with RegNr: {vehicle.RegNumber} at SpotNr: {Garage[i].SpotNumber}[/]\n");

                return; // Fordonet parkerades framgångsrikt
            }
        }
        // Ingen ledig plats hittades
        AnsiConsole.MarkupLine($"[red]No available spot found for {vehicle.VehicleType} with RegNr: {vehicle.RegNumber}\nGarage is full![/]\n[Grey]Press any key to return...[/]");
        Console.ReadKey();
        return;
    }

    // === Sök efter fordon i garaget med regnr, return spotnumber (-1 hittade ingen match) === \\
    public int SearchVehicleByRegNumber(string regNumber)
    {
        int spotNumber = -1;
        foreach (var spot in Garage)
        {
            if (spot.CheckForRegNumber(regNumber))
            {
                spotNumber = spot.SpotNumber;
                return spotNumber;
            }

        }
        return spotNumber;
    }

    /* Hämta ut ett fordon
    * Flytta ett fordon
    * 
      
    * Vi behöver även några privata hjälpmetoder:
    * Hitta ledig plats för ett fordon
    * Skapa ett fordon
    * Ta fram ett fordon, givet ett regnummer (variant på sökning)
    */


}