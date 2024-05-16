using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Storage;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TelegramActionContext : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public TelegramRequest TelegramRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public TelegramContext TelegramContext { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        internal TelegramActionContext(TelegramContext telegramContext, TelegramRequest telegramRequest)
        {
            ServiceProvider = telegramContext.ServiceProvider;
            TelegramRequest = telegramRequest;

            Session = ServiceProvider.GetRequiredService<ISession>();
            ChatId = telegramRequest.ChatId;
            TelegramBotClient = telegramRequest.TelegramBotClient;
        }

        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get;  }

        internal IExecutor Executor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient TelegramBotClient { get;  }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() =>
            Session.Dispose();
    }
}
