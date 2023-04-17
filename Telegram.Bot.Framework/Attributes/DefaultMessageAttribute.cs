using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Attributes
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
        public MessageType MessageType { get; }

        /// <summary>
        /// 默认的消息处理
        /// </summary>
        /// <param name="MessageType">将要处理的消息类型</param>
        public DefaultMessageAttribute(MessageType MessageType)
        {
            this.MessageType = MessageType;
        }
    }
}
