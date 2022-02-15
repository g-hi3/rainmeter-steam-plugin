namespace SteamFileReader;

public interface ISteamFileParsingHandler
{
    void StartDocument();
    void EndDocument();
    void StartObject(SteamFileLineData lineData);
    void EndObject(SteamFileLineData lineData);
    void ReadNameValue(SteamFileLineData lineData);
    void ReadObjectKey(SteamFileLineData lineData);
    void ReadInvalidLine(SteamFileLineData lineData);
    SteamObject GetSteamObject();
}