namespace DataAccessLibrary;

using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Spectre.Console;

public class MinaFiler
{


    


    // Metod för att ladda Configdata från en JSON-fil
    public static T? LoadFromFile<T>(string fileName)
    {
        
        try
        {
            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (FileNotFoundException)
        {

            AnsiConsole.MarkupLine($"[red]File not found.[/]");
            //AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            
            return default;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return default;
        }
    }

    // Metod för att spara data till en JSON-fil
    public static void SaveToFile<T>(string fileName, T data)
    {
        // lägger till indentering för bättre läsbarhet
        var options = new JsonSerializerOptions { WriteIndented = true };

        string jsonString = JsonSerializer.Serialize(data, options);
        File.WriteAllText(fileName, jsonString);

    }
}
