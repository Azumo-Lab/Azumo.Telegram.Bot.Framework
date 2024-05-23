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
//
//  Author: 牛奶

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.InternalCore.Install;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ICallBackManager))]
    internal class CallBackManager : ICallBackManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        private readonly Dictionary<int, string> callBackIds =
            new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public CallBackManager(IServiceProvider serviceProvider) => 
            this.serviceProvider = serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonResult"></param>
        /// <returns></returns>
        public InlineKeyboardButton CreateCallBackButton(ActionButtonResult buttonResult)
        {
            var hashCode = buttonResult.Delegate.GetHashCode();
            if (!callBackIds.TryGetValue(hashCode, out var callbackData))
            {
                callbackData = $"c{Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty)}";

                callbackData = callbackData[..30];

                var manager = serviceProvider.GetRequiredService<ICommandManager>();
                TypeDescriptor.AddAttributes(buttonResult.Delegate.Method, new BotCommandAttribute(callbackData));

                var executor = Factory.GetExecutorInstance(EnumCommandType.Func);

                executor.Analyze(
                    buttonResult.Delegate.Method,
                    buttonResult.Delegate.Target);

                manager.AddExecutor(executor);

                callBackIds.Add(hashCode, callbackData);
            }

            return InlineKeyboardButton.WithCallbackData(buttonResult.Text, callbackData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramRequest"></param>
        /// <returns></returns>
        public IExecutor? GetCallBack(TelegramRequest telegramRequest)
        {
            var data = telegramRequest.CallbackQuery?.Data;
            if (string.IsNullOrEmpty(data))
                return null;

            var manager = serviceProvider.GetRequiredService<ICommandManager>();
            return manager.GetExecutor($"/{data}");
        }
    }
}
