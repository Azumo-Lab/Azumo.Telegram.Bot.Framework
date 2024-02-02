using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Filters;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.Users;
internal class UserFilterContextFactory : IContextFactory
{
    public TelegramUserContext? GetOrCreateUserContext(IServiceProvider botServiceProvider, Update update)
    {
        var filters = botServiceProvider.GetServices<IUpdateFilter>();
        foreach (var filter in filters)
        {
            if (!filter.Filter(update))
                return null;
        }

        return null;
    }
}
