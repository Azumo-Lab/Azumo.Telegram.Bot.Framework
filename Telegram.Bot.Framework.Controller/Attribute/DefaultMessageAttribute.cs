using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Attribute
{
    /// <summary>
    /// 默认消息处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DefaultMessageAttribute : System.Attribute
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public UpdateType UpdateType { get; }
        public DefaultMessageAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
