using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Authentication.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticationAttribute : System.Attribute
    {
        public BotCommandScopeType BotCommandScopeType { get; }
        public ChatId? ChatId { get; set; }

        public ChatUser ChatUser { get; set; }

        public AuthenticationAttribute(BotCommandScopeType botCommandScopeType)
        {
            BotCommandScopeType = botCommandScopeType;
        }
    }

    public enum ChatUser
    {
        Admin,
        User,
    }
}
