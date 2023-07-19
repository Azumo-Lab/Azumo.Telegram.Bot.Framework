using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    internal class InternalUserManager : IUserManager
    {
        public int UserCount { get; set; }

        private readonly Dictionary<long, InternalChat> __ChatCache = new();
        public IChat CreateIChat(ITelegramBotClient botClient, Update update, IServiceScope BotService)
        {
            Chat chat = update.GetChat();
            if (chat == null)
                return null;

            if (__ChatCache.TryGetValue(chat.Id, out InternalChat iChat))
            {
                iChat.Request = new InternalRequest(update);
                iChat.ChatInfo.SendUser = update.GetSendUser();
            }
            else
            {
                iChat = new InternalChat(botClient, update, BotService);
                __ChatCache.TryAdd(chat.Id, iChat);
                UserCount++;
            }
            return iChat;
        }
    }
}
