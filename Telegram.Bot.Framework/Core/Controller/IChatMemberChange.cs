﻿using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChatMemberChange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramUserContext"></param>
        /// <param name="newChatMember"></param>
        /// <param name="fromUser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ChatMemberChangeAsync(TelegramUserContext telegramUserContext, ChatMember newChatMember, ChatId fromUser, ChatId user);
    }
}
