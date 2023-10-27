using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IAuthenticate))]
    internal class Authenticate : IAuthenticate
    {
        public HashSet<string> RoleName { get; set; }
    }
}
