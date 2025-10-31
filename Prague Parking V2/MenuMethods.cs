using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console;
using Prague_Parking_V2.Models;
using DataAccessLibrary;
using Prague_Parking_V2;


namespace Prague_Parking_V2;

// Menyrelaterade metoder och styrning av programmets flöde
public class MenuMethods
{
    // Visa huvudmenyn med
    public void MainMenu(ParkingGarage garage, Config config, PriceConfigData priceConfig)
    {
        bool exit = false;

        while (exit == false)
        {
            Console.Clear();
            //Rubrik med FigletText från Spectre
            AnsiConsole.Write(
                new FigletText("Prague Parking 2")
                .Color(Color.Cyan1)
                .LeftJustified()
                );
            //Visa kompakt garageöversikt och skapar en linje med Rule från Spectre
            Rule rulelineOw = new Rule("[white]Garage Overview[/]");
            AnsiConsole.Write(
                rulelineOw
                .LeftJustified()
                );
            garage.DisplayCompactGarageOverview();
            Console.WriteLine();
            Rule ruleline = new Rule("[white]Main Menu[/]");
            AnsiConsole.Write(
                ruleline
                .LeftJustified()
                );

            //Skapar en menyprompt med Spectre.Console
            string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(" [grey]Navigate with Arrowkeys, select with Enterkey[/]")
            .PageSize(7)
            .WrapAround(true)
            .AddChoices(
                "1. Park Vehicle",
                "2. Checkout Vehicle",
                "3. Move Vehicle",
                "4. Search for Vehicle",
                "5. Show Parked Vehicles",
                "6. Load Price List",
                "7. Exit"
                )
            );


            switch (choice[0])
            {
                case '1':
                    // === Call method to park vehicle === \\
                    MenuParkVehicle(garage, config);
                    //spara garage till fil efter parkering
                    ParkingGarage.SaveGarageToFile(garage);
                    AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
                    Console.ReadKey();
                    break;
                case '2':
                    // === Call method to retrieve vehicle === \\
                    MenuCheckoutVehicle(garage, priceConfig);
                    ParkingGarage.SaveGarageToFile(garage);
                    break;
                case '3':
                    // === Call method to move vehicle === \\
                    MenuMoveVehicle(garage);
                    ParkingGarage.SaveGarageToFile(garage);
                    AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
                    Console.ReadKey();
                    break;
                case '4':
                    // === Call method to search vehicle === \\
                    SearchForVehicle(garage);
                    break;
                case '5':
                    // Kalla metod för att visa parkerade fordon
                    ShowParkedVehicles(garage);
                    break;
                case '6':
                    // Call method to load price list
                    LoadAndPrintPriceList(priceConfig);
                    break;
                case '7':
                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold green]Thanks for using Prague Parking 2.0[/]");
                    exit = true;
                    return;
            }
        }
    }
    // ===== Other menu methods  ===== \\

    // === 1 Parkera fordon === \\
    public static void MenuParkVehicle(ParkingGarage garage, Config config)
    {
        //Fråga anv efter fordonstyp
        Console.Clear();
        string[] vehicleTypes = new string[] { "Car", "MC" };
        string choiceVehicleType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select what Vehicle Type you want to Park:")
            .WrapAround(true)
            .AddChoices(vehicleTypes)
            );

        //Fråga anv efter registreringsnummer
        string regNr = GetRegNrFromUser();

        Console.Clear();
        //Skapa fordon baserat på val
        if (choiceVehicleType == "Car")
        {
            Car vehicleCar = new Car(regNr, config);
            garage.ParkVehicle(vehicleCar);
        }
        else if (choiceVehicleType == "MC")
        {
            Mc vehicleMc = new Mc(regNr, config);
            garage.ParkVehicle(vehicleMc);
        }
    }

    // === 2 Checka ut fordon === \\
    public static void MenuCheckoutVehicle(ParkingGarage garage, PriceConfigData priceConfig)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[underline white]Vehicle Checkout[/]\n");
        //Fråga anv efter registreringsnummer att checka ut
        string regNrToCheckout = GetRegNrFromUser();

        int spotIndex = garage.SearchVehicleByRegNumber(regNrToCheckout);

        //Checka ut fordon från garaget
        Vehicle? vehicleToCheckout = garage.RetrieveAndRemoveVehicle(regNrToCheckout);

        if (vehicleToCheckout != null)
        {
            Console.Clear();
            // Hämta information om vart fordonet kan hämtas och hur mycket tid det har stått parkerat
            string parkingDuration = vehicleToCheckout.CalculateParkingDuration();

            // Beräkna parkeringsavgift
            decimal parkingCost;

            if (vehicleToCheckout.VehicleType == VehicleType.Car)
            {
                Car carVehicle = vehicleToCheckout as Car;
                parkingCost = carVehicle.CalculateParkingCostCar(priceConfig);
            }
            else // MC
            {
                Mc mcVehicle = vehicleToCheckout as Mc;
                parkingCost = mcVehicle.CalculateParkingCostMc(priceConfig);
            }
            AnsiConsole.MarkupLine($"Get {vehicleToCheckout.VehicleType} with Registration Number: [yellow]{regNrToCheckout}[/] at Spot Number: [yellow]{spotIndex}[/].");
            AnsiConsole.MarkupLine($" -   Parking Duration: [lime]{parkingDuration}[/]");
            AnsiConsole.MarkupLine($" - Total Parking Cost: [lime]{parkingCost:F2} CZK[/]");
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
            Console.ReadKey();
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[red]Vehicle with Registration Number [bold]{regNrToCheckout}[/] not found in the garage.[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return to main menu...[/]");
            Console.ReadKey();
        }

    }

    // === 3 Flytta fordon === \\
    //  save garage to file 
    public static void MenuMoveVehicle(ParkingGarage garage)
    {
        Console.Clear();
        //Fråga anv efter regsnummer på fordon att flytta
        AnsiConsole.MarkupLine("[underline white]Move Vehicle[/]\n");
        string regNrToMove = GetRegNrFromUser();

        //Sök efter fordon i garaget om det finns, hämta dess nuvarande plats.
        int currentSpotNumber = garage.SearchVehicleByRegNumber(regNrToMove);
        //Vehicle? vehicleToMove = garage.GetVehicleByRegNumber(regNrToMove);

        //Om fordonet inte finns i garaget
        if (currentSpotNumber == -1)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[red]Vehicle with Registration Number [bold]{regNrToMove}[/] not found in the garage.[/]");
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
            Console.ReadKey();
            return;
        }
        Console.Clear();
        //Fråga anv efter ny plats att flytta fordonet till
        AnsiConsole.MarkupLine($"\n[grey](Desired new spot number must be a number from 1 to {garage.Size})[/]\n");
        int newSpotNumber = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter Spot Number to move to:")
            .Validate(spotNr =>
            {
                if (spotNr < 1 || spotNr > garage.Size)
                {
                    return ValidationResult.Error("[red]Invalid Spot Number. Please enter a valid spot number.[/]");
                }
                return ValidationResult.Success();
            })
            );
        //Kalla på metod som flyttar om de finns plats på den nya platsen
        bool moveSuccessful = garage.MoveVehicleToAnotherSpot(regNrToMove, currentSpotNumber, newSpotNumber);
        if (moveSuccessful)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[green]Get Vehicle with Registration Number: [bold]{regNrToMove}[/] from Spot Number: [bold]{currentSpotNumber}[/] and move to Spot Number: [bold]{newSpotNumber}[/].[/]");
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
            Console.ReadKey();
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[red]Could not move Vehicle to Spot Number: [bold]{newSpotNumber}[/]. Not enough space available.[/]");
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
            Console.ReadKey();
        }
    }

    // === 4 Sök efter fordon === \\
    public static void SearchForVehicle(ParkingGarage garage)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[underline white]Search for Vehicle[/]\n");
        //Fråga anv efter registreringsnummer att söka efter
        string regNrToSearch = GetRegNrFromUser();
        Console.Clear();
        //Sök efter fordon i garaget
        int spotIndex = garage.SearchVehicleByRegNumber(regNrToSearch);
        if (spotIndex != -1)
        {

            AnsiConsole.MarkupLine($"[green]Vehicle with Registration Number: [bold]{regNrToSearch}[/] found at Spot Number [bold]{spotIndex}[/].[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Vehicle with Registration Number [bold]{regNrToSearch}[/] not found in the garage.[/]");
        }
        AnsiConsole.MarkupLine("\n[grey]Press any key to return to main menu...[/]");
        Console.ReadKey();

    }

    // === 5 Visa parkerade fordon === \\
    public static void ShowParkedVehicles(ParkingGarage garage)
    {
        Console.Clear();
        garage.DisplayParkedVehicles();
        AnsiConsole.MarkupLine("[grey]Press any key to return to main menu...[/]");
        Console.ReadKey();
    }

    // === 6 Ladda prislista och skriv ut till anv === \\
    public static void LoadAndPrintPriceList(PriceConfigData priceConfig)
    {
        //Ladda prislista från fil
        priceConfig = PriceConfigData.LoadPriceConfigFromFile(priceConfig);

        //Skriv ut prislista till konsolen
        Console.Clear();
        //Skapa en tabell med Spectre.Console
        Table table = new Table();
        table.AddColumn("Vehicle Type");
        table.AddColumn("Price per hour (CZK)");
        table.Title("[underline cyan1]Current Pricelist[/]");
        table.Border = TableBorder.Rounded;
        table.BorderColor(Color.Grey);
        table.ShowRowSeparators();
        table.AddRow("Car", priceConfig.Car.ToString("F2"));
        table.AddRow("MC", priceConfig.MC.ToString("F2"));
        AnsiConsole.Write(table);
        //Skriv ut gratis parkeringstid i en panel
        string panelString = $"[lime]First {priceConfig.FreeParkingMinutes} minutes are free of charge.[/]";
        Panel panel = new(panelString);
        panel.Border = BoxBorder.Rounded;
        panel.BorderColor(Color.Grey);
        AnsiConsole.Write(panel);
        AnsiConsole.MarkupLine("[grey]Press any key to return to main menu...[/]");
        Console.ReadKey();


    }

    // === Get regNr from user === \\
    public static string GetRegNrFromUser()
    {
        string regNr = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [yellow]Registration Number[/]:")
            .Validate(regNr =>
            {
                // Kolla om string innehåller whitespace eller är tom
                if (string.IsNullOrWhiteSpace(regNr) || regNr.Any(char.IsWhiteSpace))
                {
                    return ValidationResult.Error("[red]Registration number cannot be empty or whitespace.[/]");
                }
                // Kolla om string är längre än 10 tecken
                if (regNr.Length > 10)
                {
                    return ValidationResult.Error("[red]Registration number cannot be longer than 10 characters.[/]");
                }
                return ValidationResult.Success();
            })
            );
        // string regNr to upper case
        regNr = regNr.ToUpper();
        return regNr;
    }

}
