// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser;
using CliArgsParser.Attributes;
using SporeDownloader.Lib;

namespace SporeDownloader.Cli;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandAtlas : CliCommandAtlas {
    public SporeServer _sporeServer = new();
        
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------

    [CliCommand<SporeDownloaderArgs>("run")]
    public void Run(SporeDownloaderArgs args) {
        if (args.User is null && args.SporecastID is null) {
            Console.WriteLine("No User or Sporecast-Id were defined");
            Environment.Exit(-1);
        }
        
    }
    
    [CliCommand<SporeDownloaderArgs>("stats")]
    public async void Stats(SporeDownloaderArgs args) {
        Console.WriteLine("ha");
        var data = await _sporeServer.GetStats();
        Console.WriteLine(data.Document?.ToString());
        Console.WriteLine(data.ToString());
    }
}