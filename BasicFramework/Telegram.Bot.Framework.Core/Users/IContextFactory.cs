using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Users;
public interface IContextFactory
{
    public TelegramUserContext? GetOrCreateUserContext(IServiceProvider botServiceProvider, Update update);
}
