using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework
{
    internal class TelegramRouteController
    {
        private TelegramContext TelegramContext { get; }
        private IServiceProvider ServiceProvider { get; }

        private Dictionary<long, ITelegramRouteUserController> ChatIDUser { get; } = new Dictionary<long, ITelegramRouteUserController>();

        public TelegramRouteController(TelegramContext context, IServiceProvider serviceProvider)
        {
            TelegramContext = context;
            ServiceProvider = serviceProvider;
        }

        public async Task StartProcess()
        {
            ITelegramRouteUserController factory = ServiceProvider.GetService<ITelegramRouteUserController>();

            long chatID = TelegramContext.ChatID;
            if (!ChatIDUser.ContainsKey(chatID))
                ChatIDUser.Add(chatID, factory);
            await ChatIDUser[chatID].Invoke(TelegramContext, ServiceProvider);
        }
    }
}
