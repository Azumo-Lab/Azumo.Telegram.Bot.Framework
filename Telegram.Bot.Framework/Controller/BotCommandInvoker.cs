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
using Telegram.Bot.Framework.Controller.Params;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.Filters;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    internal class BotCommandInvoker : IExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectFactory"></param>
        /// <param name="func"></param>
        /// <param name="paramList"></param>
        /// <param name="attributes"></param>
        public BotCommandInvoker(ObjectFactory objectFactory, Func<object, object?[], object?> func, List<IGetParam> paramList, Attribute[] attributes)
        {
            _objectFactory = objectFactory;
            _func = func;
            Parameters = paramList;
            Attributes = attributes;
#if NET8_0_OR_GREATER
            Cache = [];
#else
            Cache = new Dictionary<string, object>();
#endif
        }

        private readonly ObjectFactory _objectFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, object?[], object?> _func;

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<IGetParam> Parameters { get; }

        /// <summary>
        /// 
        /// </summary>
        public Attribute[] Attributes { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Cache { get; }

        /// <summary>
        /// 
        /// </summary>
        private readonly string GUID = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public async Task<object?> Invoke(TelegramActionContext telegramActionContext)
        {
            var instance = _objectFactory(telegramActionContext.ServiceProvider, Array.Empty<object>());

            var paramManager = telegramActionContext.ServiceProvider.GetRequiredService<IParamManager>();

            object? result = null;
            if (instance is TelegramController controller)
            {
                controller.OnActionExecutionAsync(telegramActionContext);
                result = _func(controller, paramManager.GetParam());
            }
            else
            {
                result = _func(instance, paramManager.GetParam());
            }
            return result is Task<object?> actionResult ? await actionResult : result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public async Task<(ControllerResult, IActionResult?)> ActionExecute(TelegramActionContext telegramActionContext)
        {
            var commandScope = telegramActionContext.CommandScopeService;
            var obj = commandScope.Session.Get(GUID);
            if (obj == null)
            {
                var authFilters = telegramActionContext.ServiceProvider.GetServices<IAuthFilter>();
                foreach (var item in authFilters)
                    if (!await item.IsAuthorizedAsync(telegramActionContext))
                        return (ControllerResult.Forbidden, null);

                var authList = Extensions.GetOrCache(this, "{6E2F4F02-D988-44B7-A1DC-F1A64C4C3DE3}", () =>
                Attributes.Where(x => x is AuthenticationAttribute)
                    .Select(x => (AuthenticationAttribute)x)
                    .ToList());

                if (!authList.IsEmpty())
                {
                    var authRoles = Extensions.GetOrCache(this, "{7F4F7BE3-228A-4922-83A7-CF1399F456FF}", () =>
                    authList.SelectMany(x => x.RoleNames).ToHashSet());

                    var user = telegramActionContext.TelegramRequest.UserPermissions;
                    if (user.Roles.IsEmpty())
                        return (ControllerResult.Unauthorized, null);
                    foreach (var item in user.Roles)
                        if (!authRoles.Contains(item))
                            return (ControllerResult.Forbidden, null);

                    commandScope.Session.Add(GUID, GUID);
                }
            }

            var paramManager = commandScope.ParamManager;
            (var read, var actionResult) = await paramManager.Read(telegramActionContext);
            return read ? (ControllerResult.Success, actionResult) : (ControllerResult.WaitParamter, actionResult);
        }
    }
}
