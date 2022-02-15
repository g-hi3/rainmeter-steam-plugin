namespace SteamFileReader;

public class SteamFileReader
{
    private readonly IBufferedFileReader _fileReader;
    private readonly ISteamLineParser _lineParser;
    private ISteamFileParsingHandler? _parsingHandler;

    public SteamFileReader(IBufferedFileReader fileReader, ISteamLineParser lineParser)
    {
        _fileReader = fileReader;
        _lineParser = lineParser;
    }

    public void SetParsingHandler(ISteamFileParsingHandler parsingHandler)
    {
        _parsingHandler = parsingHandler;
    }
    
    public SteamObject Read(string fileName)
    {
        if (_parsingHandler == null)
        {
            throw new InvalidOperationException(
                $"There is no parsing handler defined! Call `{nameof(SetParsingHandler)}` before `{nameof(Read)}`");
        }
        
        _parsingHandler.StartDocument();
        foreach (var line in _fileReader.ReadLines(fileName))
        {
            var lineData = _lineParser.Parse(line);
            if (lineData.IsObjectStart)
            {
                _parsingHandler.StartObject(lineData);
            }
            else if (lineData.IsObjectEnd)
            {
                _parsingHandler.EndObject(lineData);
            }
            else if (lineData.IsObjectNameOnly)
            {
                _parsingHandler.ReadObjectKey(lineData);
            }
            else if (lineData.IsNameValue)
            {
                _parsingHandler.ReadNameValue(lineData);
            }
            else
            {
                _parsingHandler.ReadInvalidLine(lineData);
            }
        }
        _parsingHandler.EndDocument();
        return _parsingHandler.GetSteamObject();
    }
}