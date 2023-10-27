using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    internal interface IControllerContext
    {
        BotCommand BotCommand { get; set; }
    }
}
