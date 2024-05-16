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
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class TextMessageResult : MessageResult
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string Text;

        /// <summary>
        /// 
        /// </summary>
        private readonly ButtonResult[]? buttonResults;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buttonResult"></param>
        public TextMessageResult(string text, ButtonResult[]? buttonResult = null)
        {
             Text = text;
            buttonResults = buttonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ExecuteResultAsync(TelegramActionContext context)
        {
            if (buttonResults == null)
            {
                await context.TelegramBotClient.SendTextMessageAsync(context.ChatId, Text);
            }
            else
            {
                var manager = context.ServiceProvider.GetRequiredService<ICallBackManager>();
                var buttonList = new List<InlineKeyboardButton>();
                foreach (var button in buttonResults)
                    buttonList.Add(manager.CreateCallBackButton(button));
                await context.TelegramBotClient.SendTextMessageAsync(context.ChatId, Text, 
                    replyMarkup: new InlineKeyboardMarkup(buttonList));
            }
        }
    }
}
