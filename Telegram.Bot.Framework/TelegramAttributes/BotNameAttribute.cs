using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 设定Telegram Bot的名字
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BotNameAttribute : Attribute
    {
        /// <summary>
        /// Bot 的名称
        /// </summary>
        public string BotName { get; }

        public BotNameAttribute(string BotName)
        {
            this.BotName = BotName;
        }
    }
}
