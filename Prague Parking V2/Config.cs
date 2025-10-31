using DataAccessLibrary;
using Prague_Parking_V2.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prague_Parking_V2;

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
    // === Metoder === \ 
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
    
    // === Load config from file === \\
    public static Config LoadConfig(Config config)
    {
        string configFilePath = "../../../config.json";
        if (File.Exists(configFilePath))
        {
            config = MinaFiler.LoadFromFile<Config>(configFilePath);
            if (config == null || config.GarageNrOfSpots <= 0 || config.VehicleTypeInfo == null)
            {
                config = new Config(); //om inläsningen misslyckas, skapa en ny default config
                AnsiConsole.MarkupLine($"[red]Failed to load config, created default config[/]");
                MinaFiler.SaveToFile<Config>(configFilePath, config); //save default to json file
            }
            AnsiConsole.MarkupLine($"[green]Config loaded successful[/]");
        }
        else
        {
            Console.WriteLine("Could not find konfigurationfile! Creating a default config");
            MinaFiler.SaveToFile<Config>(configFilePath, config); //save default to json file 
        }
        return config;
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
