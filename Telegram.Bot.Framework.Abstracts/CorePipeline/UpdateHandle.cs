//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IUpdateHandler))]
    internal class UpdateHandle(IServiceProvider serviceProvider, ILogger<UpdateHandle> logger) : IUpdateHandler
    {
        private readonly IServiceProvider __ServiceProvider = serviceProvider;
        private readonly ILogger<UpdateHandle> __Logger = logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            __Logger.LogError(exception, "发生错误");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatManager = __ServiceProvider.GetRequiredService<IChatManager>();
            var telegramUserChatContext = chatManager.Create(botClient, update, __ServiceProvider);
            try
            {
                var pipelineController = telegramUserChatContext.UserScopeService.GetRequiredService<IPipelineController<TelegramUserChatContext>>();
                _ = await pipelineController.SwitchTo(telegramUserChatContext.Type, telegramUserChatContext);
            }
            catch (Exception ex)
            {
                __Logger.LogError(ex, "发生错误");
            }
        }
    }
}
