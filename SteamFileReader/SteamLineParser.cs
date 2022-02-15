using System.Text.RegularExpressions;

namespace SteamFileReader;

public class SteamLineParser : ISteamLineParser
{
    private static readonly Regex ObjectNameLine = new(
        "^(?<indent>\\t*)\"(?<name>[^\"]+)\"$",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture,
        TimeSpan.FromMilliseconds(1));
    private static readonly Regex ObjectStartLine = new(
        "^(?<indent>\\t*)\\{$",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture,
        TimeSpan.FromMilliseconds(1));
    private static readonly Regex ObjectEndLine = new(
        "^(?<indent>\\t*)}$",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture,
        TimeSpan.FromMilliseconds(1));
    private static readonly Regex NameValueLine = new(
        "^(?<indent>\\t*)\"(?<name>[^\"]+)\"\\t{2}\"(?<value>[^\"]+)\"$");
    
    public SteamFileLineData Parse(string line)
    {
        var objectNameMatch = ObjectNameLine.Match(line);
        if (objectNameMatch.Success)
        {
            return new SteamFileLineData
            {
                IsObjectNameOnly = true,
                Name = objectNameMatch.Groups["name"].Value
            };
        }

        var objectStartMatch = ObjectStartLine.Match(line);
        if (objectStartMatch.Success)
        {
            return new SteamFileLineData { IsObjectStart = true };
        }

        var objectEndMatch = ObjectEndLine.Match(line);
        if (objectEndMatch.Success)
        {
            return new SteamFileLineData { IsObjectEnd = true };
        }

        var nameValueMatch = NameValueLine.Match(line);
        if (nameValueMatch.Success)
        {
            return new SteamFileLineData
            {
                IsNameValue = true,
                Name = nameValueMatch.Groups["name"].Value,
                Value = nameValueMatch.Groups["value"].Value
            };
        }

        return SteamFileLineData.Invalid;
    }
}