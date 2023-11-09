using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TGChat : Update
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider UserService => __UserServiceScope.ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceScope __UserServiceScope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScope"></param>
        /// <param name="chatId"></param>
        private TGChat(IServiceScope serviceScope, ChatId chatId)
        {
            __UserServiceScope = serviceScope;
            ChatId = chatId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramBot"></param>
        /// <param name="chatId"></param>
        /// <param name="BotService"></param>
        /// <returns></returns>
        public static TGChat GetChat(ITelegramBotClient telegramBot, ChatId chatId, IServiceProvider BotService)
        {
            TGChat chat = new(BotService.CreateScope(), chatId)
            {
                BotClient = telegramBot,
            };
            return chat;
        }
    }
}
