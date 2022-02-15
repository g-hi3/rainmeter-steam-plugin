namespace SteamFileReader;

public interface ISteamLineParser
{
    SteamFileLineData Parse(string line);
}