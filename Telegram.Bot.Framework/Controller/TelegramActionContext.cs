using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TelegramActionContext
    {
        /// <summary>
        /// 
        /// </summary>
        private TelegramActionContext()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; set; } = null!;
    }
}
