using Prague_Parking_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class ParkingSpot
    {
        // Properties
        public int SpotNumber { get; set; } // P-platsens nummer
        public int SpotSize { get; set; } = 4; // Storlek på P-platsen i antal parkeringsenheter
        public int AvailableSize { get; set; } = 4; // Tillgänglig storlek på P-platsen

        public List<Vehicle> ParkedVehicles { get; set; } // Lista över parkerade fordon på parkeringsplatsen

        // Constructor
        public ParkingSpot(int spotNumber)
        {
            SpotNumber = spotNumber;
            AvailableSize = SpotSize;
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
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"SpotNr: {SpotNumber}, ParkedVehicles: {ParkedVehicles.Count}, AvailableSize: {AvailableSize}");
            foreach (var vehicle in ParkedVehicles)
            {
                
                info.AppendLine($" Type: {vehicle.VehicleType}, RegNr: {vehicle.RegNumber}, Size: {vehicle.Size}, ArrivalTime: {vehicle.ArrivalTime}, PricePerHour: {vehicle.PricePerHour}");
            }
            return info.ToString();
        }

    }
}