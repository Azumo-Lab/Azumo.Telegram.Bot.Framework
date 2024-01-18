using HtmlAgilityPack;

namespace Azumo.Mushi.ProcessBases;

public abstract class SearchTagProcess : BaseProcess
{
    protected override async Task<bool> Process(DataContext dataContext)
    {
        const string ALINK = nameof(ALINK);
        if (!(dataContext.Data.TryGetValue(ALINK, out var obj) && obj is List<string> atagList))
        {
            _ = dataContext.Data.Remove(ALINK);
            _ = dataContext.Data.TryAdd(ALINK, atagList = []);
        }
        foreach (var item in dataContext.Document)
            try
            {
                atagList.AddRange(await Search(item));
            }
            catch (Exception)
            {
                continue;
            }
        return true;
    }

    protected abstract Task<List<string>> Search(HtmlDocument htmlDocument);

    protected static HtmlNodeCollection SearchNodes(HtmlNode htmlNode, string xPath) =>
        htmlNode.SelectNodes($"{htmlNode.XPath}{xPath}");

    protected static HtmlNodeCollection SearchTags(HtmlNode htmlNode, string tagType)
    {
        var xpath = htmlNode.NodeType == HtmlNodeType.Document ? string.Empty : htmlNode.XPath;
        return htmlNode.SelectNodes($"{xpath}//{tagType}");
    }
}
