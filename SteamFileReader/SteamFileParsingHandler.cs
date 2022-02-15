namespace SteamFileReader;

public class SteamFileParsingHandler : ISteamFileParsingHandler
{
    private readonly Stack<string> _objectNames = new();
    private readonly Stack<SteamObject> _objectStack = new();

    private SteamObject CurrentObject => _objectStack.Peek();

    public void StartDocument()
    {
        var rootObject = new SteamObject();
        _objectStack.Push(rootObject);
        _objectNames.Push(string.Empty);
    }

    public void EndDocument()
    {
    }

    public void StartObject(SteamFileLineData lineData)
    {
        var steamObject = new SteamObject();
        _objectStack.Push(steamObject);
    }

    public void EndObject(SteamFileLineData lineData)
    {
        var steamObject = _objectStack.Pop();
        var objectName = _objectNames.Pop();
        CurrentObject.Add(objectName, steamObject);
    }

    public void ReadNameValue(SteamFileLineData lineData)
    {
        CurrentObject.Add(lineData.Name, lineData.Value);
    }

    public void ReadObjectKey(SteamFileLineData lineData)
    {
        var objectName = lineData.Name;
        _objectNames.Push(objectName);
    }

    public void ReadInvalidLine(SteamFileLineData lineData)
    {
    }

    public SteamObject GetSteamObject()
    {
        return _objectStack.Pop();
    }
}