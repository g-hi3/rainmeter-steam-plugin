namespace SteamFileReader.IntegrationTests;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting SteamFileReader.IntegrationTests ...");
        Console.WriteLine("=======================================");

        if (args.Length < 2)
        {
            throw new InvalidOperationException(
                "Argument count is less than expected! Please provide the file name to read and an expected value for /AppState/name!");
        }
        
        Console.WriteLine($"Reading file: {args[0]}");
        Console.WriteLine($"/AppState/Name should be: {args[1]}");

        var fileReader = new BufferedFileReader();
        var lineParser = new SteamLineParser();
        var steamFileReader = new SteamFileReader(fileReader, lineParser);
        var savParsingHandler = new SteamFileParsingHandler();
        steamFileReader.SetParsingHandler(savParsingHandler);

        var steamObject = steamFileReader.Read(args[0]);
        var appStateName = steamObject.Get<string>("/AppState/name");
        if (appStateName != args[1])
        {
            throw new Exception(
                $"Expected /AppState/name in file {args[0]} should have been {args[1]}, but is actually {appStateName}!");
        }
        
        Console.WriteLine("Check was successful!");
        Console.WriteLine("Finished SteamFileReader.IntegrationTests without errors.");
    }
}