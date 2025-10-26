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
        
        


        /*
        * Parkera ett fordon*/
        public void ParkVehicle(Vehicle vehicle)
        {
            
            for (int i = 0; i < Garage.Count; i++)
            {
                if (Garage[i].IsThereRoomForVehicle(vehicle))
                {
                    Garage[i].AddVehicle(vehicle);
                    AnsiConsole.MarkupLine($"[green]{vehicle.GetType().Name} with RegNr: {vehicle.RegNumber} parked at SpotNr: {Garage[i].SpotNumber}[/]");
                    return; // Fordonet parkerades framgångsrikt
                }
            }
            AnsiConsole.MarkupLine($"[red]No available spot found for {vehicle.GetType().Name} with RegNr: {vehicle.RegNumber}\nGarage is full![/]");
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