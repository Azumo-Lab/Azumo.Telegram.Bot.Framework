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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalInterface;

/// <summary>
/// 
/// </summary>
public abstract class BaseControllerParam : IControllerParam, IControllerParamSender
{
    /// <summary>
    /// 
    /// </summary>
    public virtual IControllerParamSender? ParamSender { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ParamAttribute? ParamAttribute { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public BaseControllerParam() =>
        ParamSender = this;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tGChat"></param>
    /// <returns></returns>
    public abstract Task<object> CatchObjs(TelegramUserChatContext tGChat);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tGChat"></param>
    /// <returns></returns>
    public virtual async Task<bool> SendMessage(TelegramUserChatContext tGChat)
    {
        await (ParamSender ?? this).Send(tGChat.BotClient, tGChat.UserChatID, ParamAttribute);
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="paramAttribute"></param>
    /// <returns></returns>
    public virtual async Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute? paramAttribute)
    {
        var name = paramAttribute?.Name ?? string.Empty;
        _ = await botClient.SendTextMessageAsync(chatId, $"请输入参数{name}的值");
    }
}
