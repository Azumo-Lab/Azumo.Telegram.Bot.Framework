using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticationAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public BotCommandScopeType BotCommandScopeType { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ChatUser ChatUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botCommandScopeType"></param>
        public AuthenticationAttribute(BotCommandScopeType botCommandScopeType)
        {
            BotCommandScopeType = botCommandScopeType;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ChatUser
    {
        Admin,
        User,
    }
}
