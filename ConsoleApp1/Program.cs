using Azumo.Mushi;
using Azumo.Mushi.ProcessBases;
using Azumo.Pipeline.Abstracts;
using Azumo.Reflection;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    [Test2233]
    internal class Program
    {
        static void Main(string[] args)
        {
            var az = AzumoReflection<Program>.Reflection();
            az.SetAttributes([new TestAttribute()]);
            var atr = az.GetAttribute<Test2233Attribute>();
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class TestAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.All)]
    public class Test2233Attribute : Attribute
    {

    }

    public class TestSprite : Spider
    {
        protected override IPipelineBuilder<DataContext> AddProcessFlow(IPipelineBuilder<DataContext> builder)
        {
            builder.AddProcedure(new GetHtmlProcess());
            builder.AddProcedure(new SearchATag());
            builder.AddProcedure(new PrintProcess());

            return builder;
        }

        protected override List<string> StartPages() => ["https://www.baidu.com"];
    }

    public class SearchATag : SearchTagProcess
    {
        protected override async Task<List<string>> Search(HtmlDocument htmlDocument)
        {
            await Task.CompletedTask;
            return SearchTags(htmlDocument.DocumentNode, "a").Cast<HtmlNode>().Select(x => "https:" + x.GetAttributeValue("href", string.Empty)).ToList();
        }
    }

    public class PrintProcess : BaseProcess
    {
        protected override async Task<bool> Process(DataContext dataContext)
        {
            const string ALINK = nameof(ALINK);
            if (!(dataContext.Data.TryGetValue(ALINK, out var obj) && obj is List<string> atagList))
            {
                dataContext.Data.Remove(ALINK);
                dataContext.Data.TryAdd(ALINK, atagList = []);
            }
            foreach (var item in atagList)
            {
                Console.WriteLine(item);
            }
            dataContext.StartURL.AddRange(atagList);
            return await Task.FromResult(true);
        }

        public override async Task<DataContext> ExecuteAsync(DataContext t, IPipelineController<DataContext> pipelineController)
        {
            await Process(t);
            return await pipelineController.SwitchTo(string.Empty, t);
        }
    }
}
