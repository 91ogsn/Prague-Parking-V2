using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class ParkingSpot
    {
        // Properties
        public int Size { get; set; }
        public int AvailableSize { get; set; }
        public int SpotNumber { get; set; }
        public List<Vehicle> ParkedVehicles { get; set; }

        // Constructor
        public ParkingSpot(int spotNumber)
        {
            SpotNumber = spotNumber;
            AvailableSize = Size;
            ParkedVehicles = new List<Vehicle>();
        }
        // Metoder
        public void AddVehicle(Vehicle vehicle)
        {
            ParkedVehicles.Add(vehicle);
            AvailableSize -= vehicle.Size;
        }
        public void RemoveVehicle(Vehicle vehicle)
        {
            ParkedVehicles?.Remove(vehicle);
            AvailableSize += vehicle.Size;
        }
        public bool IsThereRoomForVehicle(Vehicle vehicle)
        {
            return (vehicle.Size <= AvailableSize);
        }
        public bool CheckForRegNumber(string regNumber)
        {

            foreach (var vehicle in ParkedVehicles)
            {
                if (vehicle.RegNumber == regNumber)
                    return true;
            }
            return false;
        }

    }
}