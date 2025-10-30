using DataAccessLibrary;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prague_Parking_V2
{
    public class PriceConfigData
    {
        // === Readonly string med instruktion för användaren att läsa i pricelist filen === \\
        public readonly string Instructions1 = "READ THIS! You can adjust the prices for Cars, Mc and free parking time. ONLY! modify the numbers, do not change the structure of the file.";
        public readonly string Instructions2 = "Car: (Price in CZK per hour), MC: (Price in CZK per hour), FreeParkingMinutes: (Number of free parking minutes),";
        public readonly string Instructions3 = "Make sure to save the file after making changes.";
        
        // Properties for pricing configuration

        public decimal Car { get; set; } = 20M; // Pris CZK per timme för bil
        public decimal MC { get; set; } = 10M; // Pris CZK per timme för MC
        public int FreeParkingMinutes { get; set; } = 10; // Gratis parkeringstid i minuter

        public PriceConfigData()
        {

        }

        // ===== Metoder ===== \\

        // === override Metod to string för att visa prisdata i konsolen === \\
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Price Configuration:");
            info.AppendLine($" - Car Price per Hour: {Car} CZK");
            info.AppendLine($" - MC Price per Hour: {MC} CZK");
            info.AppendLine($" - Free Parking Minutes: {FreeParkingMinutes} minutes");
            return info.ToString();
        }
        // === Save price config to file === \\
        public static void SavePriceConfigToFile(PriceConfigData priceConfig)
        {
            string priceListFilePath = "../../../pricelist.json";
            MinaFiler.SaveToFile<PriceConfigData>(priceListFilePath, priceConfig);
        }

        // === Load price config from file === \\
        public static PriceConfigData LoadPriceConfigFromFile(PriceConfigData priceConfig)
        {
            string priceListFilePath = "../../../pricelist.json";
            if (File.Exists(priceListFilePath))
            {
                priceConfig = MinaFiler.LoadFromFile<PriceConfigData>(priceListFilePath);

                if (priceConfig == null || priceConfig.Car < 0 || priceConfig.MC < 0 || priceConfig.FreeParkingMinutes < 0)
                {
                    priceConfig = new PriceConfigData(); //om inläsningen misslyckas, skapa en ny default prislista
                    AnsiConsole.MarkupLine($"[red]Failed to load pricelist, created default pricelist[/]");
                    MinaFiler.SaveToFile<PriceConfigData>(priceListFilePath, priceConfig); //save defaultPricelist to json file
                }
                else
                {
                    AnsiConsole.MarkupLine($"[green]Pricelist loaded successful[/]");
                }
            }
            else
            {
                Console.WriteLine("Could not find pricelist file! Creating a default config");
                priceConfig = new PriceConfigData();
                MinaFiler.SaveToFile<PriceConfigData>(priceListFilePath, priceConfig); //save defaultPricelist to json file 
            }
            return priceConfig;
        }

    }
}