using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    public class BotCommand
    {
        private string __BotCommandName = string.Empty;
        public string BotCommandName
        {
            get
            {
                return __BotCommandName;
            }
            set
            {
                string botCommandName = string.Empty;
                if (!value.StartsWith('/'))
                    botCommandName = $"/{value}";
                __BotCommandName = botCommandName;
            }
        }

        public MessageType MessageType { get; set; } = MessageType.Unknown;

        public Func<TelegramController, object[], Task> Func { get; set; } = (controller, objs) => Task.CompletedTask;

        public MethodInfo MethodInfo { get; set; } = null!;

        public Type Controller { get; set; } = null!;

        public List<IControllerParam> ControllerParams { get; set; } = new();
    }
}
