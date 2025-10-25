using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class ParkingGarage
    {
        // Properties
        public List<ParkingSpot> Garage { get; set; } //TODO: Vi behöver läsa in konfiguration från fil och spara här i något lämpligt objekt
        public Config Config;
        public int Size { get; set; }
        // Constructor
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
        

        /* TODO: P-huset behöver publika metoder för:
        * Parkera ett fordon
        * Hämta ut ett fordon
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