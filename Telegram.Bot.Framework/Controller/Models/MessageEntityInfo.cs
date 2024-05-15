using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MessageEntityInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Value { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public MessageEntityType Type { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public User? User { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Language { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string? CustomEmojiId { get; internal set; }
    }
}
