﻿//  <Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalInterface
{
    public abstract class BaseControllerParam : IControllerParam, IControllerParamSender
    {
        public virtual IControllerParamSender ParamSender { get; set; }
        public ParamAttribute ParamAttribute { get; set; }

        public BaseControllerParam()
        {
            if (this is IControllerParamSender sender)
                ParamSender = sender;
        }

        public abstract Task<object> CatchObjs(TGChat tGChat);

        public virtual async Task SendMessage(TGChat tGChat, ParamAttribute paramAttribute) => await (ParamSender ?? this).Send(tGChat.BotClient, tGChat.ChatId, paramAttribute);
        public virtual async Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute paramAttribute)
        {
            var name = paramAttribute?.Name ?? string.Empty;
            _ = await botClient.SendTextMessageAsync(chatId, $"请输入参数{name}的值");
        }
    }
}