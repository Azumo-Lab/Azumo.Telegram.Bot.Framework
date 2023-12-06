using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Framework.Exec;

namespace MyChannel.Services
{
    /// <summary>
    /// 关于爬虫相关的： <see href="https://github.com/dotnetcore/DotnetSpider"/>
    /// </summary>
    /// <param name="ServiceProvider"></param>
    [DebuggerDisplay($"服务：{nameof(PublishService)}, 处理间隔：{{TimeSpan}}")]
    internal class PublishService(IServiceProvider ServiceProvider) : AbsTimedTasks
    {
        /// <summary>
        /// 
        /// </summary>
        protected override TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task Exec()
        {
            await DataBaseUpdate();
            await Spider();
        }

        /// <summary>
        /// 数据库的定期更新
        /// </summary>
        private Task DataBaseUpdate()
        {
            static List<T> Get<T>(MyDBContext context) where T : DBBase => context.Set<T>().Where(x => x.WaitingForUpdate).ToList();

            using (var serviceScope = ServiceProvider.CreateScope())
            {
                var telegramBotClient = serviceScope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                var context = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();
                var messageInfos = Get<MessageInfoEntity>(context);
                //foreach (MessageInfo messageInfo in messageInfos)
                //    _ = await telegramBotClient.EditMessageTextAsync(messageInfo.ChatID, messageInfo.MessageID, messageInfo.HtmlContent ?? string.Empty, ParseMode.Html);
            }

            return Task.CompletedTask;
        }

        private async Task<List<YandeImage>> Spider()
        {
            var yandere = new Yandere();
            var ok = await yandere.Login("", "");
            return null!;
        }

        #region 爬虫处理

        #region Yande.re 的图片爬虫
        protected class YandeImage
        {

        }

        public class Yandere
        {
            private readonly HttpClient httpClient = new(new HttpClientHandler()
            {
                Proxy = new WebProxy(""),
                UseProxy = true,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
            });

            public async Task<bool> Login(string username, string password)
            {
                var httpResponseMessage = await httpClient.GetAsync("https://yande.re/user/login", HttpCompletionOption.ResponseHeadersRead);

                if (!httpResponseMessage.IsSuccessStatusCode)
                    return false;

                var html = await httpResponseMessage.Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var postForm = htmlDoc.DocumentNode.SelectSingleNode("//form[@method='post']");
                var authToken = postForm.SelectSingleNode("//input[@name='authenticity_token']").GetAttributeValue("value", string.Empty);

                var loginMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"https://yande.re{postForm.GetAttributeValue("action", string.Empty)}")
                {
                    Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                    {
                        new("authenticity_token", authToken),
                        new("user[name]", username),
                        new("user[password]", password),
                        new("url", string.Empty),
                        new("commit", "Login"),
                    })
                });

                return loginMessage.StatusCode == HttpStatusCode.OK;
            }

            public async Task SearchImage()
            {
                var postHtml = await (await httpClient.GetAsync("https://yande.re/post?page=1")).Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(postHtml);

                var list = htmlDoc.DocumentNode.SelectSingleNode("//ul[@id='post-list-posts']");
                foreach (var node in list.SelectNodes("//li").ToList())
                {
                    var link = node.SelectSingleNode("//a[@class='thumb']").GetAttributeValue("href", string.Empty);
                    var previewLink = node.SelectSingleNode("//img[@class='preview']").GetAttributeValue("src", string.Empty);
                }
            }
        }
        #endregion

        #endregion
    }

    public class Yandere
    {
        private readonly HttpClient httpClient = new(new HttpClientHandler()
        {
            //Proxy = new WebProxy(""),
            //UseProxy = true,
            UseCookies = true,
            CookieContainer = new CookieContainer(),
        });

        private readonly DirectoryInfo IMAGE = new("C:\\Users\\ko--o\\Desktop\\IMAGE");

        public Yandere()
        {
            if (!IMAGE.Exists)
            {
                IMAGE.Create();
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            var httpResponseMessage = await httpClient.GetAsync("https://yande.re/user/login", HttpCompletionOption.ResponseHeadersRead);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return false;

            var html = await httpResponseMessage.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var postForm = htmlDoc.DocumentNode.SelectSingleNode("//form[@method='post']");
            var authToken = postForm.SelectSingleNode("//input[@name='authenticity_token']").GetAttributeValue("value", string.Empty);

            var loginMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"https://yande.re{postForm.GetAttributeValue("action", string.Empty)}")
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                    {
                        new("authenticity_token", authToken),
                        new("user[name]", username),
                        new("user[password]", password),
                        new("url", string.Empty),
                        new("commit", "Login"),
                    })
            });

            return loginMessage.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// 罗列图片信息
        /// </summary>
        /// <returns></returns>
        public async Task SearchImage()
        {
            var postHtml = await (await httpClient.GetAsync("https://yande.re/post?page=1")).Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(postHtml);

            var list = htmlDoc.DocumentNode.SelectNodes("//ul[@id='post-list-posts']//child::li").ToList();
            foreach (var node in list)
            {
                var link = $"https://yande.re{node.SelectSingleNode($"{node.XPath}//a[@class='thumb']").GetAttributeValue("href", string.Empty)}";
                var previewLink = node.SelectSingleNode($"{node.XPath}//img[@class='preview']").GetAttributeValue("src", string.Empty);

                var directoryInfo = IMAGE.CreateSubdirectory(Path.GetFileName(link));
                File.WriteAllText(Path.Combine(directoryInfo.FullName, "URL.txt"), link);
                using (var stream = new FileStream(Path.Combine(directoryInfo.FullName, Path.GetFileName(previewLink)), FileMode.OpenOrCreate))
                {
                    var downloadStream = await (await httpClient.GetAsync(previewLink)).Content.ReadAsStreamAsync();
                    await downloadStream.CopyToAsync(stream);
                }
                Console.WriteLine($"完成{link}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SYSLIB1045:转换为“GeneratedRegexAttribute”。", Justification = "<挂起>")]
        public async Task Download()
        {
            foreach (var item in IMAGE.GetDirectories())
            {
                var fileInfo = item.GetFiles("URL.txt", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (fileInfo == null)
                    continue;

                var html = await (await httpClient.GetAsync(fileInfo.OpenText().ReadToEnd())).Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var downloadNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='sidebar']//*[text()='Options']//parent::div");
                var htmlNode = downloadNode.SelectNodes($"{downloadNode.XPath}//a")
                    .Where(x => Regex.IsMatch(x.InnerText, "download", RegexOptions.IgnoreCase))
                    .Select(x =>
                    {
                        var match = Regex.Match(x.InnerText, "\\((.*)MB\\)", RegexOptions.IgnoreCase);
                        if (match.Success)
                            if (decimal.TryParse(match.Groups[1].Value, out var mb))
                                return (mb, x);
                        return (0!, null!);
                    })
                    .Where(x => x.x != null)
                    .OrderBy(x => x.mb)
                    .Select(x => x.x)
                    .FirstOrDefault();
                if (htmlNode == null)
                    continue;

                var downloadLink = htmlNode.GetAttributeValue("href", string.Empty);

                try
                {
                    var fileName = HttpUtility.UrlDecode(Path.GetFileName(downloadLink)).Replace('\0', ' ');
                    using (var filestream = new FileStream(Path.Combine(item.FullName, fileName), FileMode.OpenOrCreate))
                    {
                        await (await (await httpClient.GetAsync(downloadLink)).Content.ReadAsStreamAsync()).CopyToAsync(filestream);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(fileInfo.FullName + " 下载失败");
                    continue;
                }
            }
        }
    }
}
