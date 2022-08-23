using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.DependencyInjection;

namespace Telegram.Bot.Framework
{
    public class TelegramBot
    {
        private readonly IServiceCollection telegramServiceCollection;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceProviderBuild serviceProviderBuild;
        internal TelegramBot(TelegramBotClient botClient, ISetUp setUp)
        {
            telegramServiceCollection = new TelegramServiceCollection();
            serviceProviderBuild = (IServiceProviderBuild)telegramServiceCollection;

            setUp.Config(telegramServiceCollection);
            serviceProvider = serviceProviderBuild.Build();
        }

        public void Start()
        {

        }
    }
}
