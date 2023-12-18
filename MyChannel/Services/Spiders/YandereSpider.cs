using Azumo.Utils;
using HtmlAgilityPack;
using MyChannel.DataBaseContext.DBModels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                File.WriteAllText(COOKIE_LIST, json);
                __HttpClient.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        private readonly CookieHttpClient __CookieHttpClient;

        private readonly DirectoryInfo JsonDir;
        private readonly DirectoryInfo ImageDir;
        private readonly DirectoryInfo PreviewImageDir;

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public YandereSpider(string? proxy = null)
        {
            // Json
            jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            __CookieHttpClient = new CookieHttpClient(proxy);

            // 文件夹
            JsonDir = new DirectoryInfo("C:\\Users\\ko--o\\Desktop\\IMAGE");
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
            if (__CookieHttpClient.HasCookie)
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
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<YandereJsonImageInfo>> ListImage(int limit = 10, int? page = null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"https://yande.re/post.json?limit={limit}");
            if (page != null)
                stringBuilder.Append($"&page={page}");

            return await ListJson<YandereJsonImageInfo>(stringBuilder.ToString());
        }

        public async Task<List<T>> ListJson<T>(string url)
        {
            var postMessage = await __CookieHttpClient.Send(http => http.GetAsync(url));

            if (!postMessage.IsSuccessStatusCode)
                return null!;

            var postMessageJson = await postMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(postMessageJson))
                return null!;

            var yandereJsonImageInfo = JsonSerializer.Deserialize<List<T>>(postMessageJson);
            return yandereJsonImageInfo!;
        }

        public async Task<List<YandereJsonImageInfo>> ListImageParent(int imageID)
        {
            var url = $"https://yande.re/post.json?tags=parent:{imageID}";

            return await ListJson<YandereJsonImageInfo>(url);
        }

        public async Task<List<YandereJsonPoolInfo>> ListPool(int poolID)
        {
            var url = $"https://yande.re/pool/show.json?id={poolID}";

            return await ListJson<YandereJsonPoolInfo>(url);
        }

        public async Task<List<YandereJsonPoolInfo>> ListPool(string? query = null, int? page = null) 
        {
            var url = "https://yande.re/pool.json?";
            if(query != null)
                url += $"&query={query}";
            if (page != null)
                url += $"&page={page}";

            return await ListJson<YandereJsonPoolInfo>(url);
        }

        public async Task<List<YandereJsonTagInfo>> ListTag(int? limit = null, int? page = null, int? id = null, string? name = null)
        {
            var url = "https://yande.re/tag.json?";
            if (limit != null)
                url += $"&limit={limit}";
            if (page != null)
                url += $"&page={page}";
            if (id != null)
                url += $"&id={id}";
            if (name != null)
                url += $"&name={name}";

            return await ListJson<YandereJsonTagInfo>(url);
        }

        /// <summary>
        /// 罗列图片信息
        /// </summary>
        /// <returns></returns>
        public async Task SearchImage()
        {
            var list = await ListImage(5) ?? [];
            foreach (var item in list)
            {
                await Download(item.FileURL!);
            }
        }

        public async Task Download(string imageURL)
        {
            var resMessage = await __CookieHttpClient.Send(http => http.GetAsync(imageURL));

            var FileName = System.IO.Path.GetFileName(HttpUtility.UrlDecode(imageURL));
            var Path = System.IO.Path.Combine(ImageDir.FullName, FileName);

            using (var fileStream = new FileStream(Path, FileMode.OpenOrCreate))
            {
                await (await resMessage.Content.ReadAsStreamAsync()).CopyToAsync(fileStream);
            }
        }

        public void Dispose() => __CookieHttpClient.Dispose();
    }

    public class YandereJsonImageInfo
    {
        /// <summary>
        /// 图片的 ID
        /// </summary>
        [JsonPropertyName("id")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("tags")]
        public string? Tags { get; set; }

        [JsonPropertyName("created_at")]
        public int Created { get; set; }

        [JsonPropertyName("updated_at")]
        public int Updated { get; set; }

        [JsonPropertyName("creator_id")]
        public int? CreatorID { get; set; }

        [JsonPropertyName("approver_id")]
        public int? ApproverID { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("change")]
        public int? Change { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("md5")]
        public string? MD5 { get; set; }

        [JsonPropertyName("file_size")]
        public long FileSize { get; set; }

        [JsonPropertyName("file_ext")]
        public string? FileExt { get; set; }

        [JsonPropertyName("file_url")]
        public string? FileURL { get; set; }

        [JsonPropertyName("is_shown_in_index")]
        public bool IsShownInIndex { get; set; }

        [JsonPropertyName("preview_url")]
        public string? PreviewURL { get; set; }

        [JsonPropertyName("preview_width")]
        public int PreviewWidth { get; set; }

        [JsonPropertyName("preview_height")]
        public int PrevireHeight { get; set; }

        [JsonPropertyName("actual_preview_width")]
        public int ActualPreviewWidth { get; set; }

        [JsonPropertyName("actual_preview_height")]
        public int ActualPreviewHeight { get; set; }

        [JsonPropertyName("sample_url")]
        public string? SampleUrl { get; set; }

        [JsonPropertyName("sample_width")]
        public int SampleWidth { get; set; }

        [JsonPropertyName("sample_height")]
        public int SampleHeight { get; set; }

        [JsonPropertyName("sample_file_size")]
        public long SampleFileSize { get; set; }

        [JsonPropertyName("jpeg_url")]
        public string? JpegURL { get; set; }

        [JsonPropertyName("jpeg_width")]
        public int JpegWidth { get; set; }

        [JsonPropertyName("jpeg_height")]
        public int JpegHeight { get; set; }

        [JsonPropertyName("jpeg_file_size")]
        public long JpegFileSize { get; set; }

        [JsonPropertyName("rating")]
        public string? Rating { get; set; }

        [JsonPropertyName("is_rating_locked")]
        public bool IsRatingLocked { get; set; }

        [JsonPropertyName("has_children")]
        public bool HasChildren { get; set; }

        [JsonPropertyName("parent_id")]
        public int? ParentID { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("is_pending")]
        public bool IsPending { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("is_held")]
        public bool IsHeld { get; set; }

        [JsonPropertyName("frames_pending_string")]
        public string? FramesPendingString { get; set; }
        //frames_pending
        [JsonPropertyName("frames_string")]
        public string? FramesString { get; set; }
        //frames
        [JsonPropertyName("is_note_locked")]
        public bool IsNoteLocked { get; set; }

        [JsonPropertyName("last_noted_at")]
        public int LastNotedAt { get; set; }

        [JsonPropertyName("last_commented_at")]
        public int LastCommentedAt { get; set; }
    }

    public class YandereJsonTagInfo
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("type")]
        public YandereImageTagType Type { get; set; }

        [JsonPropertyName("ambiguous")]
        public bool Ambiguous { get; set; }
    }

    public class YandereJsonPoolInfo
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonConverter(typeof(DateTime))]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(DateTime))]
        [JsonPropertyName("updated_at")]
        public DateTime UpdateAt { get; set; }

        [JsonPropertyName("user_id")]
        public int UserID { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("posts")]
        public List<YandereJsonImageInfo> Posts { get; set; } = [];
    
    }
}
