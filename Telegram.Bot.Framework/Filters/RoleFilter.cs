//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Controller;

namespace Telegram.Bot.Framework.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RoleFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        async Task<bool> IFilter.InvokeAsync(TelegramActionContext context, IExecutor executor) =>
            !executor.Cache.TryGetValue(Extensions.RolesKey, out var roleObject)
            || !(roleObject is List<string> roleList)
            || await RoleCheck(context.Session.GetRoles().ToArray(), roleList.ToArray());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="executorRoles"></param>
        /// <returns></returns>
        protected abstract Task<bool> RoleCheck(string[] userRoles, string[] executorRoles);
    }
}
