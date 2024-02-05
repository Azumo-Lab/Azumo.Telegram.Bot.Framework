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
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.SimpleAuthentication.Users;

/// <summary>
/// 
/// </summary>
[DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IUserManager))]
internal class UserManager : IUserManager
{
    private const string RoleKey = "{79973381-B239-4CD6-8F22-544FE7053866}";
    public void ClearRole(TelegramUserContext userContext) => 
        userContext.Session.Remove(RoleKey);

    public void SetRole(TelegramUserContext userContext, string roleName)
    {
        var list = userContext.Session.Get<List<string>>(RoleKey);
        list ??= [];
        list.Add(roleName);
        userContext.Session.AddOrUpdate(RoleKey, list);
    }
    public bool Verify(TelegramUserContext userContext, string roleName)
    {
        var list = userContext.Session.Get<List<string>>(RoleKey);
        if (list == null)
            return false;

        foreach (var item in list)
            if (item.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                return true;

        return false;
    }
}
