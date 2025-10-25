using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class PriceConfigData
    {
        
        // Properties for pricing configuration
        public decimal Car { get; set; } = 20M; // Pris CZK per timme för bil
        public decimal MC { get; set; } = 10M; // Pris CZK per timme för MC
        public int FreeParkingMinutes { get; set; } = 10; // Gratis parkeringstid i minuter

        public PriceConfigData()
        {
            
        }
        // override Metod to string för att visa prisdata
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Price Configuration:");
            info.AppendLine($" - Car Price per Hour: {Car} CZK");
            info.AppendLine($" - MC Price per Hour: {MC} CZK");
            info.AppendLine($" - Free Parking Minutes: {FreeParkingMinutes} minutes");
            return info.ToString();
        }
    }
}