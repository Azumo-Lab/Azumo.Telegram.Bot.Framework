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
            TelegramRequest _TelegramRequest;
            TelegramContext _TelegramContext = null!;
            TelegramActionContext _TelegramActionContext;

            IServiceScope? _ScopeService = null;

            try
            {
                // 创建请求
                _TelegramRequest = new TelegramRequest(update, botClient);

                // 请求过滤器
                foreach (var item in requestFilters)
                    if (!item.Filter(_TelegramRequest))
                        return;

                // 创建Telegram上下文
                if (_TelegramRequest.NeedTelegramContext)
                {
                    _TelegramContext = contextFactory.GetOrCreateUserContext(BotServiceProvider, _TelegramRequest)!;
                    if (_TelegramContext == null)
                    {
                        // 出现错误，暂时屏蔽掉这个用户
                        return;
                    }
                }
                else
                {
                    _ScopeService = BotServiceProvider.CreateScope();
                    _TelegramContext = contextFactory.GetOneTimeUserContext(_ScopeService, _TelegramRequest);
                }

                // 创建Telegram动作上下文
                _TelegramActionContext = new TelegramActionContext(_TelegramContext, _TelegramRequest, cancellationToken);

                // 获取用户的流水线
                var pipeline = _TelegramActionContext.ServiceProvider.GetRequiredService<IPipelineController<TelegramActionContext, Task>>();

                // 执行流水线
                await pipeline[_TelegramRequest.Type].Invoke(_TelegramActionContext);
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
#if DEBUG
                // 如果是调试模式，将异常抛出
                throw;
#endif
            }
            finally
            {
                if (_ScopeService != null)
                {
                    _ScopeService.Dispose();
                    _TelegramContext?.Dispose();
                }
            }
        }
    }
}
