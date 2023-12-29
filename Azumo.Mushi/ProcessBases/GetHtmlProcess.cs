using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Mushi.ProcessBases
{
    public class GetHtmlProcess : BaseProcess
    {
        protected override async Task<bool> Process(DataContext dataContext)
        {
            foreach (var item in dataContext.StartURL)
            {
                try
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(await GetHtml(item));

                    dataContext.Document.Add(htmlDoc);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return true;
        }
    }
}
