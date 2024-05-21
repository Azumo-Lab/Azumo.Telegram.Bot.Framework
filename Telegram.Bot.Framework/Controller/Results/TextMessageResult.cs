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
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class TextMessageResult : ActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly TelegramMessageBuilder _Text;

        /// <summary>
        /// 
        /// </summary>
        private readonly ActionButtonResult[]? _ButtonResults;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buttonResults"></param>
        public TextMessageResult(TelegramMessageBuilder text, ActionButtonResult[]? buttonResults = null)
        {
            _Text = text;
            _ButtonResults = buttonResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="BotClient"></param>
        /// <param name="ServiceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Message[]> ExecuteResultAsync(TelegramActionContext context, ITelegramBotClient BotClient, IServiceProvider ServiceProvider, CancellationToken cancellationToken)
        {
            if (_ButtonResults == null)
            {
                var resultMessage = await BotClient.SendTextMessageAsync(context.ChatId!, _Text.ToString(), parseMode: _Text.ParseMode, cancellationToken: cancellationToken);
                return new Message[] { resultMessage };
            }
            else
            {
                var manager = ServiceProvider.GetRequiredService<ICallBackManager>();
                var buttonList = new List<InlineKeyboardButton>();
                foreach (var button in _ButtonResults)
                    buttonList.Add(manager.CreateCallBackButton(button));

                var resultMessage = await BotClient.SendTextMessageAsync(context.ChatId!, _Text.ToString(), parseMode: _Text.ParseMode,
                    replyMarkup: new InlineKeyboardMarkup(buttonList), cancellationToken: cancellationToken);

                return new Message[] { resultMessage };
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken) => 
            await context.TelegramBotClient.SendChatActionAsync(context.ChatId!, ChatAction.Typing, cancellationToken: cancellationToken);
    }
}
