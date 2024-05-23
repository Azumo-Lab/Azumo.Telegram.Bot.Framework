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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class ComboaMessageResult : ActionResult<Message[]>
    {
        private readonly List<IActionResult> items = new List<IActionResult>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ComboaMessageResult(params IActionResult[] items)
        {
            if (items == null || items.Length == 0)
                throw new ArgumentNullException(nameof(items));

            this.items.AddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken) =>
            await context.TelegramBotClient.SendChatActionAsync(context.ChatId!, ChatAction.Typing, cancellationToken: cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IRequest<Message[]> ExecuteResultAsync(TelegramActionContext context) =>
            throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task ExecuteResultAsync(TelegramActionContext context, CancellationToken cancellationToken)
        {
            await ExecuteChatActionAsync(context, cancellationToken);
            foreach (var item in items)
                await item.ExecuteResultAsync(context, context.CancellationToken);
        }
    }
}
