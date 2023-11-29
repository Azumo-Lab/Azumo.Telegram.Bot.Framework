using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Framework.Interfaces;

namespace MyChannel.Services
{
    internal class TimeingService(IServiceProvider ServiceProvider) : AbsTimerExec
    {
        protected override TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(30);

        protected override async Task Exec()
        {
            DataBaseUpdate();
            await Task.CompletedTask;
        }

        private void DataBaseUpdate()
        {
            static List<T> Get<T>(MyDBContext context) where T : DBBase
            {
                return context.Set<T>().Where(x=>x.WaitingForUpdate).ToList();
            }
            using (IServiceScope serviceScope = ServiceProvider.CreateScope())
            {
                ITelegramBotClient telegramBotClient = serviceScope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                
                MyDBContext context = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();
                List<MessageInfo> messageInfos = Get<MessageInfo>(context);
                foreach (MessageInfo messageInfo in messageInfos)
                {
                    telegramBotClient.EditMessageTextAsync(messageInfo.ChatID, messageInfo.MessageID)
                }
            }
        }
    }
}
