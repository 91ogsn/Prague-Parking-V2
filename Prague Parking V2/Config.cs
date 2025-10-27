using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using Prague_Parking_V2.Models;

namespace Prague_Parking_V2
{
    public class Config
    {
        

        // Properties
        public int GarageNrOfSpots { get; set; } = 100; // Antal P-platser för garaget
        public Dictionary<string, VehicleTypeInfo> VehicleTypeInfo { get; set; }
        

        public Config()
        {
            GarageNrOfSpots = 100;
            VehicleTypeInfo = new Dictionary<string, VehicleTypeInfo>
            {
                { "Car", new VehicleTypeInfo { SizeRequired = 4, VehiclesPerSpot = 1 } },
                { "MC", new VehicleTypeInfo { SizeRequired = 2, VehiclesPerSpot = 2 } }
            };

        }
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"GarageNrOfSpots: {GarageNrOfSpots}");
            info.AppendLine("VehicleTypeInfo:");
            foreach (var type in VehicleTypeInfo)
            {
                info.AppendLine($"  {type.Key}: SizeRequired = {type.Value.SizeRequired}, VehiclesPerSpot = {type.Value.VehiclesPerSpot}");
            }
            return info.ToString();

        }

    }
    public class VehicleTypeInfo
    {
        public int SizeRequired { get; set; } // Storlek på fordonet i antal parkeringsenheter
        public int VehiclesPerSpot { get; set; } // Antal fordon per P-plats

    }
    public enum VehicleType
    {
        Car,
        MC
        
    }
}
