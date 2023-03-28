using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public sealed class TelegramUser : User
    {
        /// <summary>
        /// 聊天窗口ID
        /// </summary>
        public long? ChatID { get; set; }
    }
}
