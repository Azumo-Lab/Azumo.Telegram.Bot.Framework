using Azumo.Utils;
using HtmlAgilityPack;
using MyChannel.DataBaseContext.DBModels;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace MyChannel.Services.Spiders
{
    internal class YandereSpider : IDisposable
    {
        private class CookieHttpClient : IDisposable
        {
            private readonly HttpClient __HttpClient;

            private readonly CookieContainer __CookieContainer;
            private const string COOKIE_LIST = nameof(COOKIE_LIST);
            public bool HasCookie { get; private set; }

            public CookieHttpClient(string? proxy = null, string? username = null, string? password = null)
            {
                // Cookie
                __CookieContainer = new CookieContainer();
                if (File.Exists(COOKIE_LIST))
                {
                    var cookie = File.ReadAllText(COOKIE_LIST) ?? string.Empty;
                    var cookieList = JsonSerializer.Deserialize<List<Cookie>>(cookie) ?? [];
                    foreach (var item in cookieList)
                        __CookieContainer.Add(item);
                    HasCookie = true;
                }
                // HttpClient
                var httpClientHandler = new HttpClientHandler()
                {
                    UseCookies = true,
                    CookieContainer = __CookieContainer,
                };
                if (!string.IsNullOrEmpty(proxy))
                {
                    httpClientHandler.Proxy = new WebProxy(proxy);
                    if (username != null && password != null)
                        httpClientHandler.Credentials = new NetworkCredential(username, password);
                }
                __HttpClient = new(httpClientHandler);
            }

            public Task<T> Send<T>(Func<HttpClient, Task<T>> SenderFunc)
            {
                ReSendTag:
                try
                {
                    return SenderFunc(__HttpClient);
                }
                catch (HttpRequestException)
                {
                    goto ReSendTag;
                }
            }

            public void Dispose()
            {
                var cookieList = __CookieContainer.GetAllCookies().Cast<Cookie>().ToList();
                var json = JsonSerializer.Serialize(cookieList);
                File.WriteAllText("COOKIE_LIST", json);
                __HttpClient.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        public class SearchOption
        {
            public const string ParentMode = nameof(ParentMode);
            public const string NULLMode = nameof(NULLMode);

            public string? URL { get; set; }
            public int? Count { get; set; }
            public int? Page { get; set; }
            public string? ParentID { get; set; }
            public string? SearchMode { get; set; } = NULLMode;
        }

        private readonly CookieHttpClient __CookieHttpClient;

        private readonly DirectoryInfo JsonDir;
        private readonly DirectoryInfo ImageDir;
        private readonly DirectoryInfo PreviewImageDir;

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public bool HasCookie { get; }

        public YandereSpider(string? proxy = null)
        {
            // Json
            jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            __CookieHttpClient = new CookieHttpClient(proxy);

            // 文件夹
            JsonDir = new DirectoryInfo("");
            ImageDir = JsonDir.CreateSubdirectory(nameof(ImageDir));
            PreviewImageDir = JsonDir.CreateSubdirectory(nameof(PreviewImageDir));
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

            var httpResponseMessage = await __CookieHttpClient.Send(http => http.GetAsync("https://yande.re/user/login"));

            if (!httpResponseMessage.IsSuccessStatusCode)
                return false;

            var html = await httpResponseMessage.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var postForm = htmlDoc.DocumentNode.SelectSingleNode("//form[@method='post']");
            var authToken = postForm.SelectSingleNode("//input[@name='authenticity_token']").GetAttributeValue("value", string.Empty);

            var loginMessage = await __CookieHttpClient.Send(http => http.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"https://yande.re{postForm.GetAttributeValue("action", string.Empty)}")
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                    {
                        new("authenticity_token", authToken),
                        new("user[name]", username),
                        new("user[password]", password),
                        new("url", string.Empty),
                        new("commit", "Login"),
                    })
            }));

            return loginMessage.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// 罗列图片信息
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Performance", "SYSLIB1045:转换为“GeneratedRegexAttribute”。", Justification = "<挂起>")]
        public async Task SearchImage(SearchOption searchOption)
        {
            switch (searchOption.SearchMode)
            {
                case SearchOption.NULLMode:
                    searchOption.URL = $"https://yande.re/post?page={searchOption.Page ?? 1}";
                    break;
                case SearchOption.ParentMode:
                    searchOption.URL = $"https://yande.re/post?tags=parent:{searchOption.ParentID ?? string.Empty}";
                    break;
                default:
                    break;
            }
            var PageMessage = await __CookieHttpClient.Send(sender => sender.GetAsync(searchOption.URL));
            var PageMessageHtml = await PageMessage.Content.ReadAsStringAsync();

            var HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(PageMessageHtml);

            var Mode = string.Empty;
            const string Mode_Parent = nameof(Mode_Parent);//母图模式

            string Parent_ID = null!;

            Match match;
            if ((match = Regex.Match(HttpUtility.UrlDecode(SearchURL), "post\\?tags=parent:(\\d*)")).Success)
            {
                Mode = Mode_Parent;
                Parent_ID = match.Groups[1].Value;
            }

            var resMessage = await ReSend(() => httpClient.GetAsync(SearchURL));
            var postHtml = await resMessage.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(postHtml);

            var list = htmlDoc.DocumentNode.SelectNodes("//ul[@id='post-list-posts']//child::li").Take(Count).ToList();

            YandeImage ParentImage = null!;
            foreach (var node in list)
            {
                var link = $"https://yande.re{node.SelectSingleNode($"{node.XPath}//a[@class='thumb']").GetAttributeValue("href", string.Empty)}";
                var previewLink = node.SelectSingleNode($"{node.XPath}//img[@class='preview']").GetAttributeValue("src", string.Empty);

                var directoryInfo = IMAGE.CreateSubdirectory(Path.GetFileName(link));
                if (directoryInfo.Name == Parent_ID)
                {
                    ParentImage = JsonSerializer.Deserialize<YandeImage>(File.ReadAllText(Path.Combine(directoryInfo.FullName, $"{nameof(YandeImage)}.JSON")))!;
                    ParentImage.ChildID = [];
                }

                if (directoryInfo.GetFiles($"{nameof(YandeImage)}.JSON", SearchOption.TopDirectoryOnly).Length != 0)
                    continue;

                string previewImagePath;
                using (var stream = new FileStream(previewImagePath = Path.Combine(directoryInfo.FullName, $"preview{Path.GetExtension(previewLink)}"), FileMode.OpenOrCreate))
                {
                    var downloadStream = await (await ReSend(() => httpClient.GetAsync(previewLink))).Content.ReadAsStreamAsync();
                    await downloadStream.CopyToAsync(stream);
                }

                var yandeImage = new YandeImage()
                {
                    DirPath = directoryInfo.FullName,
                    URL = link,
                    PreviewImagePath = previewImagePath,
                    PreviewImageURL = previewLink,
                    ParentID = Parent_ID,
                };

                if (ParentImage != null)
                {
                    ParentImage.ChildID!.Add(directoryInfo.Name);
                }

                File.WriteAllText(Path.Combine(directoryInfo.FullName, $"{nameof(YandeImage)}.JSON"), JsonSerializer.Serialize(yandeImage, jsonSerializerOptions));
            }
        }

        /// <summary>
        /// 下载图片，以及相关的信息
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Performance", "SYSLIB1045:转换为“GeneratedRegexAttribute”。", Justification = "<挂起>")]
        public async Task<List<YandeImage>> Download()
        {
            List<YandeImage> yandeImages = [];
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

                    var html = await (await ReSend(() => httpClient.GetAsync(yandeImage.URL))).Content.ReadAsStringAsync();
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    // 添加HTML文件
                    yandeImage.FileName_Path.ReplaceAdd("HTML", html.ReturnFilePathWriteTo(Path.Combine(item.FullName, $"IMAGE Time {DateTime.Now:yyyy-MM-dd HH-mm-ss}.HTML")));

                    var downloadNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='sidebar']//*[text()='Options']//parent::div");
                    var htmlNode = downloadNode.SelectNodes($"{downloadNode.XPath}//a")
                        .Where(x => Regex.IsMatch(x.InnerText, "download", RegexOptions.IgnoreCase))
                        .Select(x =>
                        {
                            var match = Regex.Match(x.InnerText, "Download.*?\\(\\s*(.*)(MB|KB)\\s*(JPG|JPEG|PNG|BMP|TIF)\\s*\\)", RegexOptions.IgnoreCase);
                            if (match.Success)
                                if (decimal.TryParse(match.Groups[1].Value, out var mb))
                                    return match.Groups[2].Value.Trim().Equals("KB", StringComparison.CurrentCultureIgnoreCase) ? (mb / 1000, x) : (mb, x);
                            return (0!, null!);
                        })
                        .Where(x => x.x != null)
                        .OrderBy(x => x.Item1)
                        .Select(x => x.x)
                        .FirstOrDefault();

                    // 添加图片下载连接
                    yandeImage.DownloadURL = htmlNode?.GetAttributeValue("href", string.Empty) ?? string.Empty;

                    if (string.IsNullOrEmpty(yandeImage.DownloadURL))
                    {
                        yandeImage.Errors.Add("未能找到下载连接，请检查HTML文件");
                        continue;
                    }

                    // 获取母图信息，子图信息，Pool信息
                    var statusNoticeList = htmlDoc.DocumentNode.SelectNodes("//div[@class='status-notice']").ToList();
                    var child = statusNoticeList.Where(x => Regex.IsMatch(x.InnerHtml, "this\\s*post\\s*has\\s*.*?child\\s*posts")).FirstOrDefault();
                    if (child != null && string.IsNullOrEmpty(child.GetAttributeValue("style", string.Empty)))
                    {
                        var childListPage = child.SelectNodes($"{child.XPath}//a").Where(x => x.GetAttributeValue("href", string.Empty).StartsWith("/post?tags")).FirstOrDefault();
                        var url = $"https://yande.re{childListPage?.GetAttributeValue("href", string.Empty)}";
                        await SearchImage(SearchURL: url);
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
                        yandeImage.Tags.Add(new TagInfo
                        {
                            TagName = tagContent?.InnerText ?? string.Empty,
                            TagTypeStr = tag.GetAttributeValue("class", string.Empty)
                        });
                    }

                    // 获取信息
                    var info = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='stats']//ul");
                    if (info != null)
                    {
                        var infoList = info.SelectNodes($"{info.XPath}//li").ToList();
                        var imageInfo = new ImageInfo
                        {
                            ID = (infoList.Select(x => Regex.Match(x.InnerText, "Id:\\s*(\\d*)")).Where(x => x.Success).Select(x => x.Groups[1].Value).FirstOrDefault() ?? string.Empty).Trim(),
                            Size = infoList.Select(x => Regex.Match(x.InnerText, "Size:\\s*(\\d*)x(\\d*)")).Where(x => x.Success).Select(x =>
                            {
                                _ = int.TryParse(x.Groups[1].Value, out var X);
                                _ = int.TryParse(x.Groups[2].Value, out var Y);
                                return new Size(X, Y);
                            }).FirstOrDefault(),
                            Rank = infoList.Select(x => Regex.Match(x.InnerText, "Rating:\\s*(.*)")).Where(x => x.Success).Select(x =>
                            {
                                return x.Groups[1].Value.ToLower() switch
                                {
                                    "safe" => YandereImageRankEntity.Green,
                                    "questionable" => YandereImageRankEntity.Yellow,
                                    "explicit" => YandereImageRankEntity.Red,
                                    _ => YandereImageRankEntity.None,
                                };
                            }).FirstOrDefault(),
                        };
                        yandeImage.ImageInfo = imageInfo;
                    }

                    // 下载图片
                    string ImagePath;
                    var fileName = HttpUtility.UrlDecode(Path.GetFileName(yandeImage.DownloadURL)).Replace('\0', ' ');
                    using (var filestream = new FileStream(ImagePath = Path.Combine(item.FullName, fileName), FileMode.OpenOrCreate))
                    {
                        // 等待1秒，防止触发反爬虫机制
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        await (await (await ReSend(() => httpClient.GetAsync(yandeImage.DownloadURL))).Content.ReadAsStreamAsync()).CopyToAsync(filestream);
                    }
                    yandeImage.ImagePath = ImagePath;
                    yandeImage.OK = true;

                    yandeImages.Add(yandeImage);
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
            return yandeImages;
        }

        public void Dispose() => __CookieHttpClient.Dispose();
    }

    public class YandeImage
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
        public List<TagInfo> Tags { get; set; } = [];

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

        public string? ParentID { get; set; }

        public List<string>? ChildID { get; set; }

        /// <summary>
        /// 是否处理完成
        /// </summary>
        public bool OK { get; set; }
    }

    public class TagInfo
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
        public YandereImageTagType TagType => (TagTypeStr?.ToLower() ?? string.Empty) switch
        {
            "tag-type-general" => YandereImageTagType.General,
            "tag-type-artist" => YandereImageTagType.Artist,
            "tag-type-copyright" => YandereImageTagType.Copyright,
            "tag-type-character" => YandereImageTagType.Character,
            "tag-type-circle" => YandereImageTagType.Circle,
            "tag-type-faults" => YandereImageTagType.Faults,
            _ => YandereImageTagType.NONE,
        };
    }

    public class ImageInfo
    {
        public string? ID { get; set; }

        public Size? Size { get; set; }

        public YandereImageRankEntity Rank { get; set; } = YandereImageRankEntity.None;
    }
}
