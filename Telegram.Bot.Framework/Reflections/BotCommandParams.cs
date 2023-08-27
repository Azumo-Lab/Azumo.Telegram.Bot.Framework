using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Reflections
{
    internal class BotCommandParams
    {
        public ParameterInfo ParameterInfo { get; set; }
        public Type MessageType { get; set; }
        public Type CatchType { get; set; }
    }
}
