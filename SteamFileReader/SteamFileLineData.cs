namespace SteamFileReader;

public struct SteamFileLineData
{
    public static readonly SteamFileLineData Invalid = new();

    public bool IsObjectStart { get; init; }
    public bool IsObjectEnd { get; init; }
    public bool IsObjectNameOnly { get; init; }
    public bool IsNameValue { get; init; }
    public string Name { get; init; }
    public object? Value { get; init; }
}