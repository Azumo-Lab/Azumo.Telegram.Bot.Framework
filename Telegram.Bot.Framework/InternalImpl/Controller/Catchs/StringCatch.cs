using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalImpl.Controller.Catchs
{
    [TypeFor(typeof(string))]
    internal class StringCatch : ICatch
    {
        public bool Catch(TGChat chat, out object obj)
        {
            string str;
            obj = null;
            bool result;
            if (result = (str = chat?.Message?.Text) != null)
                obj = str;
            return result;
        }
    }
}
