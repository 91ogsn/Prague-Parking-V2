using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class Mc : Vehicle
    {
        public Mc(string regNumber) : base(regNumber)
        {
            Size = 1; //TODO: Size skall hämtas från konfigurationsfil
            PricePerHour = 10; //TODO: PridPerHour skall hämtas från prislista
        }


    }
}