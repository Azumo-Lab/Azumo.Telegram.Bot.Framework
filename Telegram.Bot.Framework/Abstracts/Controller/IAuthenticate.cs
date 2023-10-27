using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    public interface IAuthenticate
    {
        public HashSet<string> RoleName { get; set; }
    }
}
