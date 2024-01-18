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

namespace Telegram.Bot.Framework.InternalInterface;

/// <summary>
/// 
/// </summary>
[DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerParamManager))]
internal class ControllerParamManager : IControllerParamManager
{
    private IControllerParam NowControllerParam = null!;
    private List<IControllerParam> __ControllerParams = [];
    private ResultEnum __NowResult = ResultEnum.NoStatus;
    private BotCommand? __BotCommand;

    private readonly List<object> _params = [];

    public object[] GetParams() => _params.ToArray();

    public async Task<ResultEnum> NextParam(TelegramUserChatContext tGChat)
    {
        switch (__NowResult)
        {
            case ResultEnum.NoStatus:
                try
                {
                    if (__ControllerParams.Count == 0)
                        return __NowResult = ResultEnum.Finish;

                    NowControllerParam = __ControllerParams.First();
                }
                finally
                {
                    if (__ControllerParams.Count != 0)
                        __ControllerParams.RemoveAt(0);
                }
                __NowResult = ResultEnum.SendMessage;
                _ = await NextParam(tGChat);
                break;
            case ResultEnum.SendMessage:
                var gotoCatchParamters = await NowControllerParam.SendMessage(tGChat);
                __NowResult = ResultEnum.ReceiveParameters;
                if (gotoCatchParamters)
                    _ = await NextParam(tGChat);
                break;
            case ResultEnum.ReceiveParameters:
                _params.Add(await NowControllerParam.CatchObjs(tGChat));
                __NowResult = ResultEnum.NextParam;
                _ = await NextParam(tGChat);
                break;
            case ResultEnum.NextParam:
            case ResultEnum.Finish:
                __NowResult = ResultEnum.NoStatus;
                _ = await NextParam(tGChat);
                break;
            default:
                break;
        }
        return __NowResult;
    }

    private void Clear()
    {
        NowControllerParam = null!;
        _params.Clear();
        __NowResult = ResultEnum.NoStatus;
        __BotCommand = null;
    }

    public BotCommand GetBotCommand() => __BotCommand!;

    public void NewBotCommandParamScope(BotCommand botCommand)
    {
        Clear();
        __BotCommand = botCommand;
        __ControllerParams = new List<IControllerParam>(__BotCommand.ControllerParams);
    }

    public void Dispose() => Clear();
}
