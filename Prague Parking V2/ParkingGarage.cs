using Prague_Parking_V2.Models;
using DataAccessLibrary;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

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

        //TODO: Save Garage to file method

        //Visa garage innehåll Spot nr kompakt med färkodning Grön = tom, Röd = full, Gul = delvis full.
        //varje rad ska vara 10 spots
        public void DisplayCompactGarageOverview()
        {
            
            AnsiConsole.MarkupLine("[lime]Green: [/]Empty, [yellow]Yellow: [/]Partially full, [red]Red: [/]Full\n");
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



        /*
        * Parkera ett fordon*/
        public void ParkVehicle(Vehicle vehicle)
        {
            
            for (int i = 0; i < Garage.Count; i++)
            {
                if (Garage[i].IsThereRoomForVehicle(vehicle))
                {
                    Garage[i].AddVehicle(vehicle);
                    AnsiConsole.MarkupLine($"[green]Park {vehicle.GetType().Name} with RegNr: {vehicle.RegNumber} at SpotNr: {Garage[i].SpotNumber}[/]\nPress any key to return...");
                    Console.ReadKey();
                    return; // Fordonet parkerades framgångsrikt
                }
            }
            AnsiConsole.MarkupLine($"[red]No available spot found for {vehicle.GetType().Name} with RegNr: {vehicle.RegNumber}\nGarage is full![/]\nPress any key to return...");
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