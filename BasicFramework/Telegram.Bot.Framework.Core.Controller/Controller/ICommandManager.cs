using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal interface ICommandManager
{
    public IExecutor GetExecutor(TelegramUserContext userContext);
}
