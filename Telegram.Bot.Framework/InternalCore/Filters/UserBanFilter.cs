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
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Authentication;
using Telegram.Bot.Framework.Core.Filters;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalCore.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IRequestFilter))]
    internal class UserBanFilter : IRequestFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBanList banList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="banList"></param>
        public UserBanFilter(IBanList banList) => this.banList = banList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public bool Filter(Update update) =>
            !banList.IsBanned(update.GetChatID());
    }
}
