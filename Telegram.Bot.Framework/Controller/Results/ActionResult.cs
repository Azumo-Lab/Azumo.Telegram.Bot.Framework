//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionResult<TResponse> : IActionResult
    {
        /// <summary>
        /// 发送的消息
        /// </summary>
        protected TelegramMessageBuilder? Text { get; set; }

        /// <summary>
        /// 发送的文件
        /// </summary>
        protected ListAsyncDisposable<Stream> Files { get; } =
#if NET8_0_OR_GREATER
            [];
#else
            new ListAsyncDisposable<Stream>();
#endif

        /// <summary>
        /// 发送的按钮
        /// </summary>
        protected List<ActionButtonResult> ButtonResults { get; } =
#if NET8_0_OR_GREATER
            [];
#else
            new List<ActionButtonResult>();
#endif

        /// <summary>
        /// 发送选项
        /// </summary>
        public ActionResultOption? Option { get; set; }

        /// <summary>
        /// 发送请求
        /// </summary>
        protected IRequest<TResponse> Request { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task ExecuteResultAsync(TelegramActionContext context, CancellationToken cancellationToken)
        {
            object? response = null;
            try
            {
                var BotClient = context.TelegramBotClient;

                await using (Files)
                {
                    // 设置输入动作
                    await ExecuteChatActionAsync(context, cancellationToken);
                    // 设置请求
                    Request = ExecuteResultAsync(context);
                    // 进行处理
                    response = await BotClient.MakeRequestAsync(Request, cancellationToken).ConfigureAwait(false);
                }

                // 处理结果
                var messagesResultHandlers = context.ServiceProvider.GetServices<IMessageResultHandler>();
                if (messagesResultHandlers != null)
                    foreach (var item in messagesResultHandlers)
                        await item.HandleResultAsync(context, response, cancellationToken);
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
        /// <returns></returns>
        protected abstract IRequest<TResponse> ExecuteResultAsync(TelegramActionContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionButtonResults"></param>
        /// <returns></returns>
        protected static List<InlineKeyboardButton> GetInlineKeyboardButtons(TelegramActionContext context, params ActionButtonResult[] actionButtonResults)
        {
            var callbackManager = context.ServiceProvider.GetRequiredService<ICallBackManager>();
            var buttons = new List<InlineKeyboardButton>();
            foreach (var item in actionButtonResults)
                buttons.Add(callbackManager.CreateCallBackButton(item));
            return buttons;
        }
    }
}
