﻿using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Framework.Exec;

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
        protected override Task Exec() => DataBaseUpdate();

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
                var messageInfos = Get<MessageInfo>(context);
                //foreach (MessageInfo messageInfo in messageInfos)
                //    _ = await telegramBotClient.EditMessageTextAsync(messageInfo.ChatID, messageInfo.MessageID, messageInfo.HtmlContent ?? string.Empty, ParseMode.Html);
            }

            return Task.CompletedTask;
        }
    }
}
