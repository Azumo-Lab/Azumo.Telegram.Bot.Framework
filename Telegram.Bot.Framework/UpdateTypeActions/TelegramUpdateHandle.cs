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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// 
    /// </summary>
    public class TelegramUpdateHandle : IUpdateHandler
    {
        public readonly IServiceProvider serviceProvider;
        private readonly Dictionary<UpdateType, AbstractActionInvoker> AbstractActionInvokersDic;
        public TelegramUpdateHandle(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            List<AbstractActionInvoker> abstractActionInvokers = this.serviceProvider.GetServices<AbstractActionInvoker>().ToList();
            AbstractActionInvokersDic = abstractActionInvokers.GroupBy(x => x.InvokeType).ToDictionary(x => x.Key, x => x.FirstOrDefault());
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
            string ErrorMessage = exception switch
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
            try
            {
                using (IServiceScope OneTimeScope = serviceProvider.CreateScope())
                {
                    //获取 | 创建 一个 TelegramUserScope
                    TelegramUser telegramUser;
                    IUserScopeManager userScopeManager = serviceProvider.GetService<IUserScopeManager>();
                    IUserScope userScope = userScopeManager.GetUserScope(telegramUser = TelegramContext.GetTelegramUser(update));

                    //根据不同的用户创建 TelegramContext
                    TelegramContext telegramContext = TelegramContextSetting(userScope, OneTimeScope, telegramUser, botClient, update, cancellationToken);

                    //根据消息类型获取 AbstractActionInvoker
                    if (AbstractActionInvokersDic.TryGetValue(update.Type, out AbstractActionInvoker abstractActionInvoker))
                        await abstractActionInvoker.Invoke(telegramContext);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 构造TelegramContext
        /// </summary>
        /// <param name="userScope"></param>
        /// <param name="OneTimeScope"></param>
        /// <param name="telegramUser"></param>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static TelegramContext TelegramContextSetting
            (
                IUserScope userScope,
                IServiceScope OneTimeScope,
                TelegramUser telegramUser,
                ITelegramBotClient botClient,
                Update update,
                CancellationToken cancellationToken
            )
        {
            TelegramContext telegramContext = userScope.GetTelegramContext();

            telegramContext.Update = update;
            telegramContext.CancellationToken = cancellationToken;
            telegramContext.BotClient = botClient;
            telegramContext.OneTimeScope = OneTimeScope.ServiceProvider;
            telegramContext.UserScope = userScope.GetUserServiceScope().ServiceProvider;
            telegramContext.TelegramUser = telegramUser;

            return telegramContext;
        }
    }
}
