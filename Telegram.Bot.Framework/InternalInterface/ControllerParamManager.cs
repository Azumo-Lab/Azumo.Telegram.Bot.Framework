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

namespace Telegram.Bot.Framework.InternalInterface
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerParamManager))]
    internal class ControllerParamManager : IControllerParamManager
    {
        private IControllerParam Now = null!;
        private List<IControllerParam> __ControllerParamsCopy = [];
        private List<IControllerParam> __ControllerParams = [];
        private ResultEnum __NowResult = ResultEnum.NoStatus;
        public List<IControllerParam> ControllerParams
        {
            get => __ControllerParams;
            set
            {
                __ControllerParams = value;
                __ControllerParamsCopy = new List<IControllerParam>(value);
            }
        }
        private BotCommand __BotCommand;

        private readonly List<object> _params = [];

        public object[] GetParams()
        {
            return _params.ToArray();
        }

        public async Task<ResultEnum> NextParam(TGChat tGChat)
        {
            switch (__NowResult)
            {
                case ResultEnum.NoStatus:
                    try
                    {
                        Now = __ControllerParamsCopy.FirstOrDefault();
                        if (Now == null)
                        {
                            return __NowResult = ResultEnum.Finish;
                        }
                    }
                    finally
                    {
                        if (__ControllerParamsCopy.Any())
                            __ControllerParamsCopy.RemoveAt(0);
                    }
                    __NowResult = ResultEnum.SendMessage;
                    _ = await NextParam(tGChat);
                    break;
                case ResultEnum.SendMessage:
                    if (Now != null)
                    {
                        await Now.SendMessage(tGChat);
                    }
                    __NowResult = ResultEnum.ReceiveParameters;
                    break;
                case ResultEnum.ReceiveParameters:
                    if (Now != null)
                    {
                        _params.Add(await Now.CatchObjs(tGChat));
                    }
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

        public void Clear()
        {
            Now = null;
            ControllerParams = [];
            _params.Clear();
            __NowResult = ResultEnum.NoStatus;
            __BotCommand = null;
        }

        public BotCommand GetBotCommand()
        {
            return __BotCommand!;
        }

        public void SetBotCommand(BotCommand botCommand)
        {
            __BotCommand = botCommand;
        }
    }
}
