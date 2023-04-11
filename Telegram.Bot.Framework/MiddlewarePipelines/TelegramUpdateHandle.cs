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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.Logger;
using Telegram.Bot.Framework.Abstract.Languages;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Abstract.Middlewares;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 框架的Handle
    /// </summary>
    internal class TelegramUpdateHandle : IUpdateHandler
    {
        private readonly ILogger Logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly Dictionary<UpdateType, IMiddlewarePipeline> MiddlewarePipelineDic;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        public TelegramUpdateHandle(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;

            UpdateType[] receiverOptions = (this.ServiceProvider.GetService<ReceiverOptions>() ?? new()).AllowedUpdates ?? Array.Empty<UpdateType>();
            Logger = this.ServiceProvider.GetService<ILogger>();
            MiddlewarePipelineDic = this.ServiceProvider.GetServices<IMiddlewarePipeline>()
                .GroupBy(x => x.InvokeType)
                // 只使用用户要使用的类型， 如果用户没有指定，那么就监听全部的类型
                .Where(x => receiverOptions.IsEmpty() || receiverOptions.Contains(x.Key))
                // 这里选取最后一个，也就是最新的，如果用户添加了一个新的流水线，那么就用用户新添加的
                .ToDictionary(x => x.Key, x => x.LastOrDefault());
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
            Logger.ErrorLog(exception.Message);

            await Task.CompletedTask;
        }

        /// <summary>
        /// 正确的执行者
        /// </summary>
        /// <param name="BotClient"></param>
        /// <param name="Update"></param>
        /// <param name="CancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken)
        {
            try
            {
                // 创造 TelegramSession
                ITelegramSession Session = TelegramSessionManager.Instance.GetTelegramSession(ServiceProvider, Update);
                if (Session.IsNull())
                    return;

                // 根据消息类型获取 AbstractActionInvoker
                if (MiddlewarePipelineDic.TryGetValue(Update.Type, out IMiddlewarePipeline MiddlewarePipeline))
                    await MiddlewarePipeline.Execute(Session);
            }
            catch (ApiRequestException Ex)
            {
                Logger.ErrorLog($"{nameof(ApiRequestException)} : {Environment.NewLine}{Ex.Message}");
            }
            catch (Exception Ex)
            {
                await HandlePollingErrorAsync(BotClient, Ex, CancellationToken);
            }
        }
    }
}
