using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2.Models
{
    public class Mc : Vehicle
    {
        
        public Mc(string regNumber, Config config) : base(regNumber)
        {
            Size = config.VehicleTypeInfo["MC"].SizeRequired; //hämtas från konfigurationsfil
                               
            VehicleType = VehicleType.MC;
        }
        // === Metoder === \\

        // === Calculate parking cost for MC === \\
        public decimal CalculateParkingCostMc(PriceConfigData priceConfig)
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
            totalCost += totalHours * priceConfig.MC;

            // Beräkna kostnaden för resterande minuter
            if (remainingMinutes > 0)
            {
                totalCost += priceConfig.MC; // Tar betalt för en hel timme även om det är mindre än en timme
            }
            return totalCost;
        }

    }
}