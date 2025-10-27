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


    }
}