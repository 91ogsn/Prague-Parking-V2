using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class Vehicle
    {
        // Properties
        public string? RegNumber { get; set; }
        public int Size { get; set; } // Storlek på fordonet i antal parkeringsenheter
        public DateTime ArrivalTime { get; set; } = DateTime.Now;
        public int PricePerHour { get; set; }

        // Constructor
        public Vehicle(string regNumber)
        {
            RegNumber = regNumber;
        }

    }
}