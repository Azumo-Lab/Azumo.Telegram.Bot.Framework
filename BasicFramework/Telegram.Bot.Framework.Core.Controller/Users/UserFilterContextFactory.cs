//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Framework.SimpleAuthentication;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.Users;

[DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IContextFactory))]
internal class UserFilterContextFactory : BaseDictionary<long, TelegramUserContext>, IContextFactory
{
    public TelegramUserContext? GetOrCreateUserContext(IServiceProvider botServiceProvider, Update update)
    {
        var filters = botServiceProvider.GetServices<IUpdateFilter>();
        foreach (var filter in filters)
        {
            if (!filter.Filter(update))
                return null;
        }

        var user = update.GetUser();
        var userID = user!.Id;

        TelegramUserContext telegramUserContext;
        if (ContainsKey(userID))
            telegramUserContext = Get(userID)!;
        else
        {
            telegramUserContext = new TelegramUserContext(botServiceProvider);
            _ = TryAdd(userID, telegramUserContext);
        }
        telegramUserContext.Copy(update);

        return telegramUserContext;
    }
}
