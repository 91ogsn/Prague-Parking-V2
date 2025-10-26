using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prague_Parking_V2;

namespace Prague_Parking_V2.Models
{
    public class Car : Vehicle
    {
        
        public Car(string regNumber, Config config, PriceConfigData priceConfig ) : base(regNumber)
        {

            Size = config.VehicleTypeInfo["Car"].SizeRequired;  //Hämta storlek från konfigurationsfil
            PricePerHour = priceConfig.Car;
            VehicleType = VehicleType.Car;

        }
    }
}