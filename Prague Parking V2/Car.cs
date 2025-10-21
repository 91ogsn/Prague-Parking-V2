using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class Car : Vehicle
    {
        public Car(string regNumber) : base(regNumber)
        {
            Size = 4; //TODO: Size skall hämtas från konfigurationsfil
            PricePerHour = 20; //TODO: PridPerHour skall hämtas från prislista
        }
    }
}