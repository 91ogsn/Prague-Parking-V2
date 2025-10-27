using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console;
using Prague_Parking_V2.Models;
using DataAccessLibrary;
using Prague_Parking_V2;

namespace Prague_Parking_V2
{
    // Menyrelaterade metoder och styrning av programmets flöde
    public class MenuMethods
    {
        // Visa huvudmenyn med
        public void MainMenu(ParkingGarage garage)
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
                Rule rulelineOw = new Rule("[grey]Garage Overview[/]");
                AnsiConsole.Write(
                    rulelineOw
                    .LeftJustified()
                    );
                garage.DisplayCompactGarageOverview();
                Rule ruleline = new Rule("[grey]Main Menu[/]");
                AnsiConsole.Write(
                    ruleline
                    .LeftJustified()
                    );

                //Skapar en menyprompt med Spectre.Console
                string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("  [grey]Choose an alternative[/]")
                .PageSize(7)
                .AddChoices(
                    "1. Park Vehicle",
                    "2. Checkout Vehicle",
                    "3. Move Vehicle",
                    "4. Search for Vehicle",
                    "5. Parking lot Overview",
                    "6. Load Price List",
                    "7. Exit"
                    )
                );


                switch (choice[0])
                {
                    case '1':
                        // Call method to park vehicle
                        break;
                    case '2':
                        // Call method to retrieve vehicle
                        break;
                    case '3':
                        // Call method to move vehicle
                        break;
                    case '4':
                        // Call method to search vehicle
                        break;
                    case '5':
                        // Call method to display garage content
                        break;
                    case '6':
                        // Call method to load price list
                        break;
                    case '7':
                        AnsiConsole.MarkupLine("[bold green]Thanks for using Prague Parking 2.0[/]");
                        exit = true;
                        return;
                }
            }
        }
    }
}