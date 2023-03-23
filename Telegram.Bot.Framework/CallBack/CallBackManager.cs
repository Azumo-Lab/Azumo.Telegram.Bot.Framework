//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.CallBack
{
    /// <summary>
    /// 
    /// </summary>
    internal class CallBackManager : ICallBackManager
    {

        private readonly Dictionary<string, Action<TelegramSession>> _CallBackDic = new Dictionary<string, Action<TelegramSession>>();

        public string CreateCallBack(Action<TelegramSession> CallBackAction)
        {
            return CreateCallBack(Guid.NewGuid().ToString(), CallBackAction);
        }

        public string CreateCallBack(string CallBackName, Action<TelegramSession> CallBackAction)
        {
            string callBackKey = Guid.NewGuid().ToString();
            _CallBackDic.Add(callBackKey, CallBackAction);
            return callBackKey;
        }

        public void Dispose(string CallbackName)
        {
            throw new NotImplementedException();
        }

        public Action<TelegramSession> GetCallBack(string CallBackKey)
        {
            throw new NotImplementedException();
        }
    }
}
