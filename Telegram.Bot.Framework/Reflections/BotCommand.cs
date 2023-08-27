using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Reflections
{
    internal class BotCommand
    {
        public string BotCommandName { get; set; }
        public MessageType? MessageType { get; set; }

        public Func<TelegramController, object[], Task> Command { get; set; } 
        public List<BotCommandParams> BotCommandParams { get; set; }
        public Type ControllerType { get; set; }
    }
}
