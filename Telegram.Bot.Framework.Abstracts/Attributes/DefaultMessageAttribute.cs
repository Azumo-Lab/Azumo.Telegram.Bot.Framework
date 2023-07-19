using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 默认消息处理
    /// </summary>
    /// <remarks>
    /// 设定默认的消息处理类型，这个类型是 <see cref="Types.Enums.MessageType"/> <br></br>
    /// 可以通过这个设定消息类型的默认处理。例如Photo类型的默认处理（用户在聊天界面直接发送一张图片）。<br></br>
    /// 在方法上面设定 <see cref="Types.Enums.MessageType.Photo"/> 类型的默认处理，如果图片没有其他程序处理，框架就会调用默认的处理。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DefaultMessageAttribute : Attribute
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; }

        /// <summary>
        /// 该指令能够使用的范围
        /// </summary>
        public ChatScopeEnum ChatScope { get; set; } = ChatScopeEnum.All;

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
