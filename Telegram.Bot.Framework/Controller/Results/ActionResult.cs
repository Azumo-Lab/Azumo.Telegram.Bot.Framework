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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        protected TelegramMessageBuilder? Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ListAsyncDisposable<Stream> Files { get; } = new ListAsyncDisposable<Stream>();

        /// <summary>
        /// 
        /// </summary>
        protected ActionResultOption Option { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionResultOption"></param>
        protected ActionResult(ActionResultOption? actionResultOption = null) => 
            Option = actionResultOption ?? new ActionResultOption();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(TelegramActionContext context, CancellationToken cancellationToken)
        {
            Message[] resultMessages;
            try
            {
                await using (Files)
                {
                    // 设置输入动作
                    await ExecuteChatActionAsync(context, cancellationToken);
                    // 进行处理
                    resultMessages = await ExecuteResultAsync(context, context.TelegramBotClient, context.ServiceProvider, cancellationToken);
                }

                // 处理结果
                var messagesResultHandlers = context.ServiceProvider.GetServices<IMessageResultHandler>();
                if (messagesResultHandlers != null)
                    foreach (var message in resultMessages)
                        foreach (var item in messagesResultHandlers)
                            await item.HandleResultAsync(context, message, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Files.Clear();
            }
        }

        /// <summary>
        /// 设置动作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="BotClient"></param>
        /// <param name="ServiceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task<Message[]> ExecuteResultAsync(TelegramActionContext context, ITelegramBotClient BotClient, IServiceProvider ServiceProvider, CancellationToken cancellationToken);
    }
}
