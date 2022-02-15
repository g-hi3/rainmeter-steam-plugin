using System;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace SteamFileReader.UnitTests;

public class SteamFileReaderTests
{
    private const string TestSteamFileName = "";
    
    [Test]
    public void Read_ParsingHandlerIsNotSet_ThrowsAnInstanceOfInvalidOperationException()
    {
        var fileReader = Mock.Of<IBufferedFileReader>();
        var lineParser = Mock.Of<ISteamLineParser>();
        var steamReader = new SteamFileReader(fileReader, lineParser);

        Assert.That(() => steamReader.Read(TestSteamFileName), Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void Read_StartDocumentIsCalledOnce()
    {
        var fileReader = Mock.Of<IBufferedFileReader>();
        var lineParser = Mock.Of<ISteamLineParser>();
        var steamReader = new SteamFileReader(fileReader, lineParser);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.StartDocument(), Times.Once);
    }

    [Test]
    public void Read_EndDocumentIsCalledOnce()
    {
        var fileReader = Mock.Of<IBufferedFileReader>();
        var lineParser = Mock.Of<ISteamLineParser>();
        var steamReader = new SteamFileReader(fileReader, lineParser);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.EndDocument(), Times.Once);
    }

    [Test]
    public void Read_LineDataIsObjectStart_CallsStartObject()
    {
        var fileReaderMock = new Mock<IBufferedFileReader>();
        fileReaderMock
            .Setup(fileReader => fileReader.ReadLines(It.IsAny<string>()))
            .Returns(Enumerable.Repeat(string.Empty, 1));
        var lineParserMock = new Mock<ISteamLineParser>();
        var lineData = new SteamFileLineData { IsObjectStart = true };
        lineParserMock
            .Setup(lineParser => lineParser.Parse(It.IsAny<string>()))
            .Returns(lineData);
        var steamReader = new SteamFileReader(fileReaderMock.Object, lineParserMock.Object);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.StartObject(lineData), Times.Once);
    }

    [Test]
    public void Read_LineDataIsObjectEnd_CallsEndObject()
    {
        var fileReaderMock = new Mock<IBufferedFileReader>();
        fileReaderMock
            .Setup(fileReader => fileReader.ReadLines(It.IsAny<string>()))
            .Returns(Enumerable.Repeat(string.Empty, 1));
        var lineParserMock = new Mock<ISteamLineParser>();
        var lineData = new SteamFileLineData { IsObjectEnd = true };
        lineParserMock
            .Setup(lineParser => lineParser.Parse(It.IsAny<string>()))
            .Returns(lineData);
        var steamReader = new SteamFileReader(fileReaderMock.Object, lineParserMock.Object);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.EndObject(lineData), Times.Once);
    }

    [Test]
    public void Read_LineDataIsNameValue_CallsReadNameValue()
    {
        var fileReaderMock = new Mock<IBufferedFileReader>();
        fileReaderMock
            .Setup(fileReader => fileReader.ReadLines(It.IsAny<string>()))
            .Returns(Enumerable.Repeat(string.Empty, 1));
        var lineParserMock = new Mock<ISteamLineParser>();
        var lineData = new SteamFileLineData { IsNameValue = true };
        lineParserMock
            .Setup(lineParser => lineParser.Parse(It.IsAny<string>()))
            .Returns(lineData);
        var steamReader = new SteamFileReader(fileReaderMock.Object, lineParserMock.Object);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.ReadNameValue(lineData), Times.Once);
    }

    [Test]
    public void Read_LineDataIsObjectKeyOnly_CallsReadObjectKey()
    {
        var fileReaderMock = new Mock<IBufferedFileReader>();
        fileReaderMock
            .Setup(fileReader => fileReader.ReadLines(It.IsAny<string>()))
            .Returns(Enumerable.Repeat(string.Empty, 1));
        var lineParserMock = new Mock<ISteamLineParser>();
        var lineData = new SteamFileLineData { IsObjectNameOnly = true };
        lineParserMock
            .Setup(lineParser => lineParser.Parse(It.IsAny<string>()))
            .Returns(lineData);
        var steamReader = new SteamFileReader(fileReaderMock.Object, lineParserMock.Object);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.ReadObjectKey(lineData), Times.Once);
    }

    [Test]
    public void Read_LineDataIsInvalid_CallsReadInvalidLine()
    {
        var fileReaderMock = new Mock<IBufferedFileReader>();
        fileReaderMock
            .Setup(fileReader => fileReader.ReadLines(It.IsAny<string>()))
            .Returns(Enumerable.Repeat(string.Empty, 1));
        var lineParserMock = new Mock<ISteamLineParser>();
        var lineData = SteamFileLineData.Invalid;
        lineParserMock
            .Setup(lineParser => lineParser.Parse(It.IsAny<string>()))
            .Returns(lineData);
        var steamReader = new SteamFileReader(fileReaderMock.Object, lineParserMock.Object);
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.ReadInvalidLine(lineData), Times.Once);
    }

    [Test]
    public void Read_CallsGetSteamObject()
    {
        var fileReader = Mock.Of<IBufferedFileReader>();
        var lineParser = Mock.Of<ISteamLineParser>();
        var steamReader = new SteamFileReader(fileReader, lineParser);
        var expected = new SteamObject();
        var parsingHandlerMock = new Mock<ISteamFileParsingHandler>();
        parsingHandlerMock
            .Setup(parsingHandler => parsingHandler.GetSteamObject())
            .Returns(expected);
        steamReader.SetParsingHandler(parsingHandlerMock.Object);

        _ = steamReader.Read(TestSteamFileName);
        
        parsingHandlerMock.Verify(parsingHandler => parsingHandler.GetSteamObject(), Times.Once);
    }
}