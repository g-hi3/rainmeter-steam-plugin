namespace SteamFileReader;

public class BufferedFileReader : IBufferedFileReader
{
    public IEnumerable<string> ReadLines(string fileName) => File.ReadLines(fileName);
}