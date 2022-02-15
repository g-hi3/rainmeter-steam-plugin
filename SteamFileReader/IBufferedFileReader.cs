namespace SteamFileReader;

public interface IBufferedFileReader
{
    public IEnumerable<string> ReadLines(string fileName);
}