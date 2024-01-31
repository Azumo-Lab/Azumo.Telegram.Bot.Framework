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

using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.Controller;

/// <summary>
/// 
/// </summary>
public abstract class BaseParameterGetter
{
    public EnumReadParam Result { get; private set; } = EnumReadParam.None;

    public void Init() =>
        Result = EnumReadParam.None;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task SendPromptMessage(TelegramUserChatContext context)
    {
        try
        {
            await Send(context);
            Result = EnumReadParam.WaitInput;
        }
        catch (Exception)
        {
            Result = EnumReadParam.OK;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public object? GetParam(TelegramUserChatContext context)
    {
        try
        {
            return GetParamObj(context);
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            Result = EnumReadParam.OK;
        }
    }

    protected abstract Task Send(TelegramUserChatContext context);
    protected abstract object? GetParamObj(TelegramUserChatContext context);
}
