using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prague_Parking_V2;

namespace Prague_Parking_V2.Models
{
    public class Car : Vehicle
    {
        
        public Car(string regNumber, Config config) : base(regNumber)
        {

            Size = config.VehicleTypeInfo["Car"].SizeRequired;  //Hämta storlek från konfigurationsfil
            VehicleType = VehicleType.Car;

        }
        // === Metoder === \\

        // === Calculate parking cost for Car === \\
        public decimal CalculateParkingCostCar(PriceConfigData priceConfig)
        {
            TimeSpan timeParked = DateTime.Now - ArrivalTime;
            decimal totalCost = 0;
            // Kontrollera om parkeringstiden är inom den fria parkeringstiden
            if (timeParked.TotalMinutes <= priceConfig.FreeParkingMinutes)
            {
                return totalCost; // Ingen kostnad inom gratis parkeringstid
            }
            // Räkna antalet hela timmar och minuter
            int totalHours = (int)timeParked.TotalHours;
            int remainingMinutes = timeParked.Minutes;

            // Beräkna kostnaden för hela timmar
            totalCost += totalHours * priceConfig.Car;
            // Beräkna kostnaden för resterande minuter
            if (remainingMinutes > 0)
            {
                totalCost += priceConfig.Car; // Tar betalt för en hel timme även om det är mindre än en timme
            }
            return totalCost;
        }
    }
}