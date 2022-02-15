using System;
using NUnit.Framework;

namespace SteamFileReader.UnitTests;

public class SteamObjectTests
{
    [Test]
    public void Get_EmptyString_ThrowsInvalidOperationException()
    {
        var steamObject = new SteamObject();
        
        Assert.That(
            () => steamObject.Get(string.Empty),
            Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void Get_KeyDoesNotExist_ThrowsInvalidOperationException()
    {
        var steamObject = new SteamObject();
        
        Assert.That(
            () => steamObject.Get("/CustomData"),
            Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void Get_KeyExists_ReturnsValueToKey()
    {
        const string key = "CustomData";
        const string expected = "hello world";
        var steamObject = new SteamObject { { key, expected } };

        var actual = steamObject.Get($"/{key}");
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Get_WithNestedPath_ReturnsValueToNestedPath()
    {
        const string outerKey = "outer";
        const string nestedKey = "inner";
        const string expected = "hello world";
        var steamObject = new SteamObject { { outerKey, new SteamObject { { nestedKey, expected } } } };

        var actual = steamObject.Get($"/{outerKey}/{nestedKey}");
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Get_FoundObjectIsSteamObject_ReturnsSteamObject()
    {
        const string key = "outer";
        var steamObject = new SteamObject { { key, new SteamObject() } };

        var foundObject = steamObject.Get($"/{key}");
        
        Assert.That(foundObject, Is.InstanceOf<SteamObject>());
    }

    [Test]
    public void Get_OfWrongType_ThrowsInvalidOperationException()
    {
        const string key = "ContainedData";
        var steamObject = new SteamObject { { key, new SteamObject() } };

        Assert.That(
            () => steamObject.Get<string>($"/{key}"),
            Throws.InstanceOf<InvalidOperationException>());
    }
}