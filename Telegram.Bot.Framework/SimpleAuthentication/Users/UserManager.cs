//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Attributes;

namespace Telegram.Bot.Framework.SimpleAuthentication.Users
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IUserManager))]
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IContextFilter))]
    internal class UserManager : IUserManager, IContextFilter
    {
        /// <summary>
        /// 
        /// </summary>

        /* 项目“Telegram.Bot.Framework (net8.0)”的未合并的更改
        在此之前:
            private const string RoleKey = "{79973381-B239-4CD6-8F22-544FE7053866}";

            /// <summary>
        在此之后:
            private const string RoleKey = "{79973381-B239-4CD6-8F22-544FE7053866}";

            /// <summary>
        */
        private const string RoleKey = "{79973381-B239-4CD6-8F22-544FE7053866}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        public void ClearRole(TelegramUserContext userContext) =>
            userContext.Session.Remove(RoleKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        public bool Filter(TelegramUserContext userContext, string[] roleNames)
        {
            if (roleNames.Length == 0)
                return true;

            var list = userContext.Session.Get<List<string>>(RoleKey);
            if (list == null)
                return false;

            var roles = new HashSet<string>(roleNames.Select(x => x.ToLower()));

            foreach (var item in list)
            {
                if (roles.Contains(item.ToLower()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="roleName"></param>
        public void SetRole(TelegramUserContext userContext, string roleName)
        {
            var list = userContext.Session.Get<List<string>>(RoleKey);
            list ??=
#if NET8_0_OR_GREATER
                [];
#else
                new List<string>();
#endif
            list.Add(roleName);
            userContext.Session.AddOrUpdate(RoleKey, list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool Verify(TelegramUserContext userContext, string roleName) =>
#if NET8_0_OR_GREATER
            Filter(userContext, [roleName]);
#else
            Filter(userContext, new string[] { roleName });
#endif

    }
}
