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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Fragments;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        protected TelegramRequest TelegramRequest { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        protected TelegramContext TelegramContext { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        protected UserPermissions User { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public virtual async Task<ControllerResult> OnActionExecutionAsync(TelegramActionContext actionContext)
        {
            TelegramRequest = actionContext.TelegramRequest;
            TelegramContext = actionContext.TelegramContext;
            User = new UserPermissions();

            // 没有权限
            var hashSet = actionContext.Executor.GetOrCache(Extensions.Auth, () =>
            {
                var list = actionContext.Executor.Attributes.Where(x => x is AuthenticationAttribute).Select(x => (AuthenticationAttribute)x).ToList();
                return list.SelectMany(x => x.RoleNames).ToHashSet();
            });
            if (User.Roles.Count == 0 && hashSet.Count != 0)
                return ControllerResult.Unauthorized;

            // 权限不正确
            var flag = false;
            foreach (var item in User.Roles)
                if (flag = hashSet.Contains(item))
                    break;
            if (!flag)
                return ControllerResult.Forbidden;

            // 获取参数
            var paramManager = actionContext.ServiceProvider.GetRequiredService<IParamManager>();
            if (!await paramManager.Read(actionContext.TelegramContext))
                return ControllerResult.WaitParamter;

            // 执行控制器
            var result = await actionContext.Executor.Invoke(this, paramManager.GetParam());
            if (result != null)
            {
                if (!(result is IActionResult actionResult))
                    actionResult = new MessageResult(new TextMessage(result.ToString()));
                await actionResult.ExecuteResultAsync(actionContext);
            }
            return ControllerResult.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual Task<IActionResult> MessageResultAsync(string message) =>
            Task.FromResult<IActionResult>(new MessageResult(new TextMessage(message)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        protected virtual Task<IActionResult> MessageResultAsync(string message, string images) =>
            Task.FromResult<IActionResult>(new MessageResult(new TextMessage(message), new ImageMessage(images)));

    }
}
