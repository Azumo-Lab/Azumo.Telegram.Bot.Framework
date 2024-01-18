using HtmlAgilityPack;

namespace Azumo.Mushi;

public abstract class DataContext
{
    public List<string> StartURL { get; } = [];

    public List<string> WaitingForProcessingURL { get; } = [];

    public Dictionary<object, object> Data { get; } = [];

    public List<HtmlDocument> Document { get; } = [];
}
