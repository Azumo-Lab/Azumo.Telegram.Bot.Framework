using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    public interface IMessage
    {
        public Task SendAsync(TGChat chat);
    }
}
