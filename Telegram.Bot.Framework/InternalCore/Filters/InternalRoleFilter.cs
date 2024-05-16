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
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Filters;

namespace Telegram.Bot.Framework.InternalCore.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IFilter))]
    internal class InternalRoleFilter : RoleFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="executorRoles"></param>
        /// <returns></returns>
        protected override Task<bool> RoleCheck(string[] userRoles, string[] executorRoles)
        {
            foreach (var item in executorRoles)
                if (userRoles.Contains(item))
                    return Task.FromResult(true);
            return Task.FromResult(false);
        }
    }
}
