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
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// 可以使用的Bot名称
        /// </summary>
        public IEnumerable<string> BotName { get; }

        /// <summary>
        /// 使用标签
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        public CommandAttribute(string CommandName)
        {
            if (!CommandName.StartsWith('/'))
            {
                CommandName = $"/{CommandName}";
            }
            this.CommandName = CommandName;
        }

        /// <summary>
        /// 使用标签
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        /// <param name="BotName">Bot名称(可以使用多个)</param>
        public CommandAttribute(string CommandName, params string[] BotName) : this(CommandName)
        {
            this.BotName = new List<string>(BotName);
        }
    }
}
