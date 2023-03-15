using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Models;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    public sealed class TelegramSession : IDisposable
    {
        public bool IsClose { get; private set; }
        public TelegramUser User { get; } = default!;

        public ISession Session { get; } = default!;

        private IServiceScope __ServiceScope;
        public IServiceProvider UserService
        {
            get
            {
                return __ServiceScope.ServiceProvider;
            }
        }

        public ITelegramBotClient BotClient { get; } = default!;

        public ITelegramBot TelegramBot { get; } = default!;

        private TelegramSession(IServiceProvider serviceProvider)
        {
            __ServiceScope = serviceProvider.CreateScope();

            TelegramBot = UserService.GetRequiredService<ITelegramBot>();
            BotClient = UserService.GetRequiredService<ITelegramBotClient>();
            Session = UserService.GetRequiredService<ISession>();
        }

        public static TelegramSession CreateSession(IServiceProvider serviceProvider)
        {
            return new TelegramSession(serviceProvider);
        }

        public void Dispose()
        {
            IsClose = true;
            __ServiceScope.Dispose();
        }
    }
}
