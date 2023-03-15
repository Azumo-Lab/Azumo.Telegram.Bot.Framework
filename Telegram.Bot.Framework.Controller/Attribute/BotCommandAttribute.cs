using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Attribute
{
    /// <summary>
    /// 机器人指令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : System.Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string CommandName { get; } = default!;

        /// <summary>
        /// 简单的描述
        /// </summary>
        public string CommandDescription { get; set; } = "没有描述";

        public BotCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
