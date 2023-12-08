using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using MyChannel.Services.Spiders;
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
            List<YandeImage> images = null!;
            var yandere = new YandereSpider();
            if (await yandere.Login("", ""))
            {
                await yandere.SearchImage();
                images = await yandere.Download();
            }
            if (images == null)
                return;
            using (var scopeService = ServiceProvider.CreateAsyncScope())
            {
                var dBContext = scopeService.ServiceProvider.GetRequiredService<MyDBContext>();
                foreach (var image in images)
                {
                    dBContext.YandereSpiderEntity.Add(new YandereSpiderEntity
                    {
                        DirPath = image.DirPath,
                        HTMLPath = image.FileName_Path.GetValueOrDefault("HTML", string.Empty),
                        ImageID = image.ImageInfo?.ID,
                        ImageSize = new YandereImageSizeEntity
                        {
                            Height = image.ImageInfo?.Size?.Height ?? 0,
                            Width = image.ImageInfo?.Size?.Width ?? 0,
                        },
                        Tags = image.Tags?.Select(x => new YandereTagsEntity
                        {
                            TagName = x.TagName,
                            TagTypeStr = x.TagTypeStr,
                            YandereImageTagType = x.TagType,
                        })?.ToList(),
                        HTMLURL = image.URL,
                        ImagePath = image.ImagePath,
                        ImageURL = image.URL,
                        PreviewImagePath = image.PreviewImagePath,
                        PreviewImageURL = image.PreviewImageURL,
                        Json = JsonSerializer.Serialize(image),
                        JsonPath = Directory.GetFiles(image.DirPath ?? Path.GetFullPath("/"), "*.JSON", SearchOption.TopDirectoryOnly).FirstOrDefault(),
                        ImageRank = image.ImageInfo?.Rank,
                    });
                }
                await dBContext.SaveChangesAsync();
            }
        }
    }
}
