//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class UpdateHandler : IUpdateHandler
    {
        public readonly IServiceProvider serviceProvider;

        public UpdateHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 错误的执行者
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

                _ => exception.ToString()
            };

            string logFile = "TelegramErrorLog.log";
            if (!System.IO.File.Exists(logFile))
                System.IO.File.Create(logFile).Close();

            using (StreamWriter sw = System.IO.File.AppendText(logFile))
            {
                await sw.WriteAsync(ErrorMessage);
                await sw.FlushAsync();
            }
        }

        /// <summary>
        /// 正确的执行者
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using (IServiceScope OneTimeScope = serviceProvider.CreateScope())
            {
                //获取 | 创建 一个TelegramUserScope
                ITelegramUserScopeManager telegramUserScopeManager = serviceProvider.GetService<ITelegramUserScopeManager>();
                ITelegramUserScope telegramUserScope = telegramUserScopeManager.GetTelegramUserScope(TelegramContext.GetChatID(update));

                TelegramContext telegramContext = telegramUserScope.CreateTelegramContext();
                telegramContext.Update = update;
                telegramContext.CancellationToken = cancellationToken;
                telegramContext.BotClient = botClient;
                telegramContext.OneTimeScope = OneTimeScope.ServiceProvider;

                await telegramUserScope.Invoke(OneTimeScope);
            }
        }
    }
}
