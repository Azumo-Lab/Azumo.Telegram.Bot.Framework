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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Filters;
using Telegram.Bot.Framework.PipelineMiddleware;
using Telegram.Bot.Framework.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IUpdateHandler))]
    internal class TelegramUpdateHandle : IUpdateHandler
    {
        public TelegramUpdateHandle(IServiceProvider serviceProvider)
        {
            BotServiceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<TelegramUpdateHandle>>();
            requestFilters = serviceProvider.GetServices<IRequestFilter>() ?? new List<IRequestFilter>();
            contextFactory = serviceProvider.GetRequiredService<IContextFactory>();
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider BotServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<TelegramUpdateHandle>? _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // 发生错误，将信息写入Log中
            _logger?.LogError("发生错误，错误类型：{A0}，错误信息：{A1}", exception.GetType().FullName, exception.ToString());
            await Task.CompletedTask;
        }

        private readonly IEnumerable<IRequestFilter> requestFilters;
        private readonly IContextFactory contextFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // 创建请求
                var request = new TelegramRequest(update);

                // 请求过滤器
                foreach (var item in requestFilters)
                    if (!item.Filter(request))
                        return;

                // 创建Telegram上下文
                var telegramContext = contextFactory.GetOrCreateUserContext(BotServiceProvider, request);
                if (telegramContext == null)
                    return;

                // 创建Telegram动作上下文
                var actionContext = new TelegramActionContext(telegramContext, request);

                // 获取用户的流水线
                var pipeline = actionContext.ServiceProvider.GetRequiredService<IPipelineController<TelegramActionContext, Task>>();

                // 执行流水线
                await pipeline[request.Type].Invoke(actionContext);
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }
            finally
            {

            }
        }
    }
}
