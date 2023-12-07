using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using NLog.Targets;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
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

        private async Task Spider()
        {
            var yandere = new Yandere();
            var ok = await yandere.Login("", "");
        }

        #region 爬虫处理

        #region Yande.re 的图片爬虫
        protected class YandeImage
        {
            /// <summary>
            /// 文件夹路径
            /// </summary>
            public string? DirPath { get; set; }

            /// <summary>
            /// 图片详细URL路径
            /// </summary>
            public string? URL { get; set; }

            /// <summary>
            /// 图片下载连接
            /// </summary>
            public string? DownloadURL { get; set; }

            /// <summary>
            /// 图片的Tag信息
            /// </summary>
            public List<Tag> Tags { get; set; } = [];

            /// <summary>
            /// 图像的信息
            /// </summary>
            public ImageInfo? ImageInfo { get; set; }

            /// <summary>
            /// 预览图像下载路径
            /// </summary>
            public string? PreviewImageURL { get; set; }

            /// <summary>
            /// 预览图像路径
            /// </summary>
            public string? PreviewImagePath { get; set; }

            /// <summary>
            /// 图片路径
            /// </summary>
            public string? ImagePath { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public List<string> Errors { get; set; } = [];

            /// <summary>
            /// 文件名称：文件路径
            /// </summary>
            public Dictionary<string, string> FileName_Path { get; set; } = [];

            /// <summary>
            /// 是否处理完成
            /// </summary>
            public bool OK { get; set; }
        }

        protected class Tag
        {
            /// <summary>
            /// Tag 的名称
            /// </summary>
            public string? TagName { get; set; }

            /// <summary>
            /// Tag 的类型
            /// </summary>
            public string? TagTypeStr { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public TagType TagType => (TagTypeStr?.ToLower() ?? string.Empty) switch
            {
                "tag-type-general" => TagType.General,
                "tag-type-artist" => TagType.Artist,
                "tag-type-copyright" => TagType.Copyright,
                "tag-type-character" => TagType.Character,
                _ => TagType.NONE,
            };
        }

        protected class ImageInfo
        {
            public string? ID { get; set; }

            public Size? Size { get; set; }
        }

        protected enum TagType
        {
            NONE,
            /// <summary>
            /// 艺术家
            /// </summary>
            Artist,
            /// <summary>
            /// 作品
            /// </summary>
            Copyright,
            /// <summary>
            /// 角色标签
            /// </summary>
            Character,
            /// <summary>
            /// 普通标签
            /// </summary>
            General,
        }

        public class Yandere : IDisposable
        {
            private readonly HttpClient httpClient;

            private readonly CookieContainer cookieContainer;

            private const string COOKIE_LIST = nameof(COOKIE_LIST);

            private readonly DirectoryInfo IMAGE = new("C:\\Users\\ko--o\\Desktop\\IMAGE");

            private readonly JsonSerializerOptions jsonSerializerOptions;

            public bool HasCookie { get; }

            public Yandere(string? proxy = null)
            {
                // Json
                jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                // Cookie
                cookieContainer = new CookieContainer();
                if (File.Exists(COOKIE_LIST))
                {
                    var cookie = File.ReadAllText(COOKIE_LIST) ?? string.Empty;
                    var cookieList = JsonSerializer.Deserialize<List<Cookie>>(cookie) ?? [];
                    foreach (var item in cookieList)
                        cookieContainer.Add(item);
                    HasCookie = true;
                }

                // HttpClient
                var httpClientHandler = new HttpClientHandler()
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer,
                };
                if (!string.IsNullOrEmpty(proxy))
                    httpClientHandler.Proxy = new WebProxy(proxy);
                httpClient = new(httpClientHandler);

                // 文件夹
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
                if (HasCookie)
                    return true;

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
                string postHtml;
                ReSend:
                try
                {
                    postHtml = await (await httpClient.GetAsync("https://yande.re/post?page=1")).Content.ReadAsStringAsync();
                }
                catch (HttpRequestException)
                {
                    goto ReSend;
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(postHtml);

                var list = htmlDoc.DocumentNode.SelectNodes("//ul[@id='post-list-posts']//child::li").ToList();
                foreach (var node in list)
                {
                    var link = $"https://yande.re{node.SelectSingleNode($"{node.XPath}//a[@class='thumb']").GetAttributeValue("href", string.Empty)}";
                    var previewLink = node.SelectSingleNode($"{node.XPath}//img[@class='preview']").GetAttributeValue("src", string.Empty);

                    var directoryInfo = IMAGE.CreateSubdirectory(Path.GetFileName(link));

                    if (directoryInfo.GetFiles($"{nameof(YandeImage)}.JSON", SearchOption.TopDirectoryOnly).Length != 0)
                        continue;
                    
                    string previewImagePath;

                    using (var stream = new FileStream(previewImagePath = Path.Combine(directoryInfo.FullName, $"preview{Path.GetExtension(previewLink)}"), FileMode.OpenOrCreate))
                    {
                        var downloadStream = await (await httpClient.GetAsync(previewLink)).Content.ReadAsStreamAsync();
                        await downloadStream.CopyToAsync(stream);
                    }

                    var yandeImage = new YandeImage()
                    {
                        DirPath = directoryInfo.FullName,
                        URL = link,
                        PreviewImagePath = previewImagePath,
                        PreviewImageURL = previewLink,
                    };

                    File.WriteAllText(Path.Combine(directoryInfo.FullName, $"{nameof(YandeImage)}.JSON"), JsonSerializer.Serialize(yandeImage, jsonSerializerOptions));
                }
            }

            /// <summary>
            /// 下载图片，以及相关的信息
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("Performance", "SYSLIB1045:转换为“GeneratedRegexAttribute”。", Justification = "<挂起>")]
            public async Task Download()
            {
                foreach (var item in IMAGE.GetDirectories())
                {
                    var infoFile = item.GetFiles($"{nameof(YandeImage)}.JSON", SearchOption.TopDirectoryOnly).FirstOrDefault();

                    StreamReader? jsonFileStream;
                    var yandeImage = JsonSerializer.Deserialize<YandeImage>((jsonFileStream = infoFile?.OpenText())?.ReadToEnd() ?? string.Empty) ?? new YandeImage();
                    jsonFileStream?.Dispose();

                    try
                    {
                        if (yandeImage.OK)
                            continue;

                        if (yandeImage.Errors.Count != 0)
                            continue;

                        // 等待1秒，防止触发反爬虫机制
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        var html = await (await httpClient.GetAsync(yandeImage.URL)).Content.ReadAsStringAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        // 添加HTML文件
                        yandeImage.FileName_Path.Add("HTML", WriteToFile(html, item, $"IMAGE Time {DateTime.Now:yyyy-MM-dd HH-mm-ss}.html"));

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

                        // 添加图片下载连接
                        yandeImage.DownloadURL = htmlNode?.GetAttributeValue("href", string.Empty) ?? string.Empty;

                        if (string.IsNullOrEmpty(yandeImage.DownloadURL))
                        {
                            yandeImage.Errors.Add("未能找到下载连接，请检查HTML文件");
                            continue;
                        }

                        // 获取TAG
                        var tagNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@id='tag-sidebar']");
                        foreach (var tag in tagNode.SelectNodes($"{tagNode.XPath}//li").ToList())
                        {
                            var tagContent = tag.SelectNodes($"{tag.XPath}//a")
                                .Select(x =>
                                {
                                    var tagStr = HttpUtility.UrlDecode(x.GetAttributeValue("href", string.Empty)).Replace("/post?tags=", string.Empty).Replace('\0', ' ');
                                    return (tagStr, x);
                                })
                                .Where(x => x.tagStr == x.x.InnerText || x.tagStr.Replace('_', ' ') == x.x.InnerText)
                                .Select(x => x.x)
                                .FirstOrDefault();
                            yandeImage.Tags.Add(new Tag
                            {
                                TagName = tagContent?.InnerText ?? string.Empty,
                                TagTypeStr = tag.GetAttributeValue("class", string.Empty)
                            });
                        }

                        // 信息
                        var info = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='stats']//ul");
                        if (info != null)
                        {
                            var infoList = info.SelectNodes($"{info.XPath}//li").ToList();
                            var imageInfo = new ImageInfo
                            {
                                ID = (infoList.Select(x=>Regex.Match(x.InnerText, "Id:\\s*(\\d*)")).Where(x => x.Success).Select(x => x.Groups[1].Value).FirstOrDefault() ?? string.Empty).Trim(),
                                Size = infoList.Select(x => Regex.Match(x.InnerText, "Size:\\s*(\\d*)x(\\d*)")).Where(x => x.Success).Select(x => 
                                {
                                    _ = int.TryParse(x.Groups[1].Value, out var X);
                                    _ = int.TryParse(x.Groups[2].Value, out var Y);
                                    return new Size(X, Y);
                                }).FirstOrDefault(),
                            };
                            yandeImage.ImageInfo = imageInfo;
                        }

                        string ImagePath;
                        var fileName = HttpUtility.UrlDecode(Path.GetFileName(yandeImage.DownloadURL)).Replace('\0', ' ');
                        using (var filestream = new FileStream(ImagePath = Path.Combine(item.FullName, fileName), FileMode.OpenOrCreate))
                        {
                            // 等待1秒，防止触发反爬虫机制
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            await (await (await httpClient.GetAsync(yandeImage.DownloadURL)).Content.ReadAsStreamAsync()).CopyToAsync(filestream);
                        }
                        yandeImage.ImagePath = ImagePath;
                        yandeImage.OK = true;
                    }
                    catch (Exception ex)
                    {
                        yandeImage.Errors.Add(ex.ToString());
                    }
                    finally
                    {
                        File.WriteAllText(Path.Combine(item.FullName, $"{nameof(YandeImage)}.JSON"), JsonSerializer.Serialize(yandeImage, jsonSerializerOptions));
                    }
                }
            }

            public async Task<string> GetTest(string url)
            {
                var resMessage = await httpClient.GetAsync(url);
                return await resMessage.Content.ReadAsStringAsync();
            }

            private static string WriteToFile(string content, DirectoryInfo directoryInfo, string fileName = "Error.txt")
            {
                var filePath = Path.Combine(directoryInfo.FullName, fileName);
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();
                File.AppendAllLines(filePath, [content]);
                return filePath;
            }

            public void Dispose()
            {
                var cookieList = cookieContainer.GetAllCookies().Cast<Cookie>().ToList();
                var json = JsonSerializer.Serialize(cookieList, jsonSerializerOptions);
                File.WriteAllText("COOKIE_LIST", json);
                httpClient.Dispose();

                GC.SuppressFinalize(this);
            }
        }
        #endregion

        #endregion
    }
}
