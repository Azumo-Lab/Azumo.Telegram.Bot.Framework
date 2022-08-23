using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramException
{
    /// <summary>
    /// 重复的命令
    /// </summary>
    public class RepeatedCommandException : Exception
    {
        public RepeatedCommandException(string CommandName) : base($"重复的命令 ：{CommandName}")
        {

        }

        public RepeatedCommandException() : base() { }
    }
}
