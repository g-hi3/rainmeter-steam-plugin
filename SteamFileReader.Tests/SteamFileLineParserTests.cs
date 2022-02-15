using NUnit.Framework;

namespace SteamFileReader.UnitTests;

public class SteamFileLineParserTests
{
    private const string ObjectNameLine = "\t\"ObjectData\"";
    private const string ObjectStartLine = "\t{";
    private const string ObjectEndLine = "\t}";
    private const string NameValueLine = "\t\"DataName\"\t\t\"DataValue\"";
    private const string InvalidLine = "\tDataName";
    private const string ValueAsIntegerLine = "\"DataName\"\t\t\"123456\"";

    [Test]
    public void Parse_LineIsObjectName_ReturnedLineDataIsObjectNameOnly()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(ObjectNameLine);
        
        Assert.That(lineData.IsObjectNameOnly);
    }

    [Test]
    public void Parse_LineIsObjectName_ReturnsLineDataWithName()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(ObjectNameLine);
        
        Assert.That(lineData.Name, Is.EqualTo("ObjectData"));
    }

    [Test]
    public void Parse_LineIsObjectStart_ReturnedLineDataIsObjectStart()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(ObjectStartLine);
        
        Assert.That(lineData.IsObjectStart);
    }

    [Test]
    public void Parse_LineIsObjectEnd_ReturnedLineDataIsObjectEnd()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(ObjectEndLine);
        
        Assert.That(lineData.IsObjectEnd);
    }

    [Test]
    public void Parse_LineIsNameValue_ReturnedLineDataIsNameValue()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(NameValueLine);
        
        Assert.That(lineData.IsNameValue);
    }

    [Test]
    public void Parse_LineIsNameValue_ReturnsLineDataWithNameAndValue()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(NameValueLine);

        var expected = new SteamFileLineData
        {
            IsNameValue = true,
            Name = "DataName",
            Value = "DataValue"
        };
        Assert.That(lineData, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_LineIsInvalid_ReturnsInvalidLineData()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(InvalidLine);
        
        Assert.That(lineData, Is.EqualTo(SteamFileLineData.Invalid));
    }

    [Test]
    public void Parse_ValueIsInteger_ReturnsValueAsString()
    {
        var lineParser = new SteamLineParser();

        var lineData = lineParser.Parse(ValueAsIntegerLine);
        
        Assert.That(lineData.Value, Is.InstanceOf<string>());
    }
}