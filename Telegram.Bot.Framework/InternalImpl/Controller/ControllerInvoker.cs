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
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Controller.Filters;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Reflections;
using Telegram.Bot.Types;
using BotCommand = Telegram.Bot.Framework.Reflections.BotCommand;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    /// <summary>
    /// 控制器执行
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerInvoker))]
    internal class ControllerInvoker : IControllerInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider __ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly List<IControllerInvokeFilter> __ControllerInvokeFilters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ControllerInvoker(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider;
            __ControllerInvokeFilters = __ServiceProvider.GetServices<IControllerInvokeFilter>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public BotCommand GetCommand(Update update)
        {
            return BotCommandRoute.GetBotCommand(update.GetCommand()) ?? BotCommandRoute.GetBotCommand(update.Message.Type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tGChat"></param>
        /// <returns></returns>
        public async Task InvokeAsync(BotCommand command, TGChat tGChat, IControllerParamManager controllerParamManager)
        {
            if (command == null) return;
            if (tGChat == null) return;

            TelegramController telegramController = (TelegramController)ActivatorUtilities.GetServiceOrCreateInstance(__ServiceProvider, command.ControllerType);
            try
            {
                foreach (IControllerInvokeFilter controllerInvokeFilter in __ControllerInvokeFilters)
                    await controllerInvokeFilter.InvokeBefore(telegramController, tGChat);

                await telegramController.ControllerInvokeAsync(tGChat, command.Command, controllerParamManager);

                foreach (IControllerInvokeFilter controllerInvokeFilter in __ControllerInvokeFilters)
                    await controllerInvokeFilter.InvokeAfter(telegramController, tGChat);
            }
            catch (Exception ex)
            {
                foreach (IControllerInvokeFilter controllerInvoke in __ControllerInvokeFilters)
                    await controllerInvoke.InvokeWhenError(telegramController, tGChat, ex);
            }
        }
    }
}
