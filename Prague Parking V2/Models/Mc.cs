using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2.Models
{
    public class Mc : Vehicle
    {
        
        public Mc(string regNumber, Config config, PriceConfigData priceConfig) : base(regNumber)
        {
            Size = config.VehicleTypeInfo["MC"].SizeRequired; //hämtas från konfigurationsfil
            PricePerHour = priceConfig.MC;                    //PridPerHour skall hämtas från prislista
            VehicleType = VehicleType.MC;
        }


    }
}