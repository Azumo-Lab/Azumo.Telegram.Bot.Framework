using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 指令标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; }
        public CommandAttribute(string CommandName)
        {
            if (!CommandName.StartsWith('/'))
            {
                CommandName = $"/{CommandName}";
            }
            this.CommandName = CommandName;
        }
    }
}
