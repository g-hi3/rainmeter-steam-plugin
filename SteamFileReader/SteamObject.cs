using System.Collections;
using System.Text;

namespace SteamFileReader;

[Serializable]
public struct SteamObject : IEnumerable
{
    private IDictionary<string, object?> Content { get; }

    public SteamObject()
    {
        Content = new Dictionary<string, object?>();
    }

    public SteamObject(IDictionary<string, object?> content)
    {
        Content = content.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void Add(string key, object? value)
    {
        Content.Add(key, value);
    }

    public object? Get(string path)
    {
        return Get<object?>(path);
    }

    public T Get<T>(string path)
    {
        var pathParts = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
        return Get<T>(pathParts);
    }

    private object? Get(IEnumerable<string> pathParts)
    {
        return Get<object?>(pathParts);
    }

    public T Get<T>(IEnumerable<string> pathParts)
    {
        var keys = pathParts.ToList();
        
        if (!keys.Any())
        {
            throw new InvalidOperationException("Invalid path!");
        }
        
        var firstKey = keys.First();

        if (!Content.ContainsKey(firstKey))
        {
            throw new InvalidOperationException($"Object doesn't contain the key {firstKey}!");
        }
        
        var nestedObject = Content[firstKey];

        if (keys.Count == 1)
        {
            if (nestedObject is T typedObject)
            {
                return typedObject;
            }

            throw new InvalidOperationException($"Value of {firstKey} is not of type {typeof(T)}");
        }

        if (nestedObject is not SteamObject steamObject)
        {
            throw new InvalidOperationException($"Object doesn't contain the key {firstKey}!");
        }
        
        var remainingKeys = keys.Skip(1);
        return steamObject.Get<T>(remainingKeys);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Content.GetEnumerator();
    }

    public static string AsPathsWithValuesString(string previousString, SteamObject steamObject)
    {
        var stringBuilder = new StringBuilder();
        
        foreach (var (name, value) in steamObject.Content)
        {
            var path = $"{previousString}/{name}";
            var appendableValue = value is SteamObject nestedObject
                ? AsPathsWithValuesString(path, nestedObject)
                : value;
            stringBuilder
                .Append(path)
                .Append(": ")
                .Append(appendableValue)
                .AppendLine();
        }

        return stringBuilder.ToString();
    }
}