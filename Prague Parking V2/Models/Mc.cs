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
            Size = config.VehicleTypeInfo["MC"].SizeRequired; //TODO: Size skall hämtas från konfigurationsfil
            PricePerHour = 10; //TODO: PridPerHour skall hämtas från prislista
        }


    }
}