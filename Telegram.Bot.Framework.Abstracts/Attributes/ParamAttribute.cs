using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamAttribute : Attribute
    { 
        public Type? ControllerParamSenderType { get; set; }
    }
}
