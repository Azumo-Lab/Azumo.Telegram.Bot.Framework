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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Users
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IAuthenticate))]
    internal class Authenticate : IAuthenticate
    {
        /// <summary>
        /// 
        /// </summary>
        public HashSet<Enum> RoleName { get; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tGChat"></param>
        /// <param name="authenticateAttribute"></param>
        /// <returns></returns>
        public Task<bool> IsAuthenticated(TelegramUserChatContext tGChat, AuthenticateAttribute authenticateAttribute)
        {
            var result = false;
            foreach (var roleName in RoleName)
                if (!(result = authenticateAttribute.RoleName.Contains(roleName)))
                    continue;
            return Task.FromResult(result);
        }
    }
}
