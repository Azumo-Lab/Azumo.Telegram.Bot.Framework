using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Attribute
{
    /// <summary>
    /// 机器人指令标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : System.Attribute
    {
        /// <summary>
        /// 指令，以 '/start' 形式发送的指令
        /// </summary>
        public string Command { get; } = default!;

        /// <summary>
        /// 指令的简单描述
        /// </summary>
        public string Description { get; set; } = "没有描述";

        /// <summary>
        /// 机器人指令标签
        /// </summary>
        /// <param name="Command">指令，以 '/start' 形式发送的指令</param>
        public BotCommandAttribute(string Command)
        {
            this.Command = Command;
        }
    }
}
