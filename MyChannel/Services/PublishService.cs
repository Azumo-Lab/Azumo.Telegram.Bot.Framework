using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Framework.Abstracts.Exec;
using Telegram.Bot.Types.Enums;

namespace MyChannel.Services
{
    /// <summary>
    /// 
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
        }

        /// <summary>
        /// 数据库的定期更新
        /// </summary>
        private async Task DataBaseUpdate()
        {
            static List<T> Get<T>(MyDBContext context) where T : DBBase
            {
                return context.Set<T>().Where(x => x.WaitingForUpdate).ToList();
            }

            using (IServiceScope serviceScope = ServiceProvider.CreateScope())
            {
                ITelegramBotClient telegramBotClient = serviceScope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                MyDBContext context = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();
                List<MessageInfo> messageInfos = Get<MessageInfo>(context);
                //foreach (MessageInfo messageInfo in messageInfos)
                //    _ = await telegramBotClient.EditMessageTextAsync(messageInfo.ChatID, messageInfo.MessageID, messageInfo.HtmlContent ?? string.Empty, ParseMode.Html);
            }
        }
    }
}
