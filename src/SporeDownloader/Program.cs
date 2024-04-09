// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using SporeDownloader.Cli;

namespace SporeDownloader;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    private static readonly ICliArgsParser _parser = new CliArgsParser.CliArgsParser()
        .RegisterFromCliAtlas(new CommandAtlas());
    
    public static void Main(string[] args) {
        Task.Run(() => _parser.TryParseAsync(args)) ;
    }
}