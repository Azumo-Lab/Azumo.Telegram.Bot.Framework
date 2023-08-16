//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Telegram.Bot.Framework.Abstracts.Process;
using Telegram.Bot.Framework.Abstracts.User;

namespace Telegram.Bot.Framework.InternalProc.Process
{
    /// <summary>
    /// 用户数量的定时更新
    /// </summary>
    [DependencyInjection<IExec>(ServiceLifetime.Singleton)]
    internal class UserCountUpdate : AbsTimedTask
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider __ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public UserCountUpdate(IServiceProvider serviceProvider)
            => __ServiceProvider = serviceProvider;

        /// <summary>
        /// 用户数量更新间隔
        /// </summary>
        /// <returns></returns>
        protected override TimeSpan GetTimeSpan() => TimeSpan.FromMinutes(10);

        /// <summary>
        /// 执行用户数量更新
        /// </summary>
        /// <returns></returns>
        protected override async Task Invoke()
        {
            IUserManager userManager = __ServiceProvider.GetService<IUserManager>();
            userManager.UpdateUserCount();
            await Task.CompletedTask;
        }
    }
}
