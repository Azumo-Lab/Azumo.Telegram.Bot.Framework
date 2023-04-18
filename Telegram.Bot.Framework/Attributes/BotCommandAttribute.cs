using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.InternalImplementation.Languages;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Attributes
{
    /// <summary>
    /// 机器人指令标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : Attribute
    {
        /// <summary>
        /// 指令，以 '/start' 形式发送的指令
        /// </summary>
        public string Command { get; } = default!;

        /// <summary>
        /// 指令的简单描述
        /// </summary>
        public string Description 
        { 
            get => __Description ?? MultiLanguageStatic.Language[ItemKey.DefaultCommandDetails];
            set
            {
                __Description = value;
            }
        }
        private string __Description;

        /// <summary>
        /// 机器人指令标签
        /// </summary>
        /// <param name="Command">指令，以 '/start' 形式发送的指令</param>
        public BotCommandAttribute(string Command)
        {
            if (!Command.StartsWith("/"))
            {
                Command = $"/{Command}";
            }
            this.Command = Command;
        }
    }
}
