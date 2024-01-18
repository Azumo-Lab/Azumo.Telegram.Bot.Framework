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

using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.UserAuthentication;

internal class RoleManager : IRoleManager
{
    private readonly HashSet<string> __HashSetRoles = [];
    private readonly Dictionary<long, (User, List<string>)> __Users = [];
    private readonly Dictionary<string, List<User>> __RoleUser = [];
    public void AddRole(string roleName) =>
        __HashSetRoles.Add(roleName);

    public void AddUser(User user, List<string> roles)
    {
        _ = roles.RemoveAll(x => !__HashSetRoles.Contains(x));
        if (!__Users.TryAdd(user.Id, (user, roles)))
            __Users[user.Id] = (user, roles);
    }

    public List<User> GetRoleUser(string roleName)
    {
        if (!VerifyRole(roleName))
            return [];

        if (!__RoleUser.TryGetValue(roleName, out var users))
        {
            users ??= [];
            users.AddRange(__Users
                .Where(x => x.Value.Item2.Contains(roleName))
                .Select(x => x.Value.Item1)
                .ToList());
        }
        return users;
    }

    public bool VerifyRole(string roleName) =>
        __HashSetRoles.Contains(roleName);
}
