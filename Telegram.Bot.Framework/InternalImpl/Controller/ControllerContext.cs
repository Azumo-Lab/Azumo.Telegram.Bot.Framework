using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    [DependencyInjection(ServiceLifetime.Transient, typeof(IControllerContext))]
    internal class ControllerContext : IControllerContext
    {
        public BotCommand BotCommand { get; set; }
    }
}
