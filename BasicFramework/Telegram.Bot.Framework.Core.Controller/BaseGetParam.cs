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

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller;
public abstract class BaseGetParam : IGetParam
{
    public ParamAttribute? ParamAttribute { get; }
    public abstract Task<object> GetParam(TelegramUserContext context);
    public virtual async Task<bool> SendMessage(TelegramUserContext context)
    {
        if (ParamAttribute?.IGetParmType != null)
            if (ActivatorUtilities.CreateInstance(context.UserServiceProvider, ParamAttribute.IGetParmType, []) is IGetParam iGetParam)
                return await iGetParam.SendMessage(context);
        _ = await context.BotClient.SendTextMessageAsync(context.ScopeChatID, ParamAttribute?.Message ?? "请输入参数");
        return false;
    }
}