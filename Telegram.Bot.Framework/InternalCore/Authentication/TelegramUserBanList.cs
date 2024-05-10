using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Authentication;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalCore.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IBanList))]
    internal class TelegramUserBanList : IBanList
    {
        /// <summary>
        /// 
        /// </summary>
        public HashSet<ChatId> ChatIds { get; } = new HashSet<ChatId>();

        public bool IsBanned(ChatId? chatId) => 
            chatId == null || ChatIds.Contains(chatId);
    }
}
