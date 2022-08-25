using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParamMakerFor : Attribute
    {
        public Type Type { get; }

        public ParamMakerFor(Type type)
        {
            Type = type;
        }
    }
}
