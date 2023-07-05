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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.InternalImplementation.CallBack
{
    /// <summary>
    /// 
    /// </summary>
    internal class InternalCallBackManager : ICallBackManager
    {
        private string __CallBackKey;
        private readonly IServiceProvider __serviceProvider;
        private readonly Dictionary<string, ICallBack> __CallBackDic = new();

        public InternalCallBackManager(IServiceProvider serviceProvider)
        {
            __serviceProvider = serviceProvider;
        }

        public string CreateCallBack(Func<ITelegramChat, Task> CallBackAction)
        {
            ICallBack callBack = new InternalCallBack(CallBackAction);
            string result = Guid.NewGuid().ToString();
            if (__CallBackDic.TryAdd(result, callBack))
                return result;
            return string.Empty;
        }

        public void Dispose(string CallBackKey)
        {
            __CallBackDic.Remove(CallBackKey);
        }

        public ICallBack GetCallBack(string CallBackKey)
        {
            return __CallBackDic.TryGetValue(CallBackKey, out ICallBack callBack) ? callBack : null;
        }

        void ICallBackManager.SetCallBackKey(string CallBackKey)
        {
            __CallBackKey = CallBackKey;
        }

        ICallBack ICallBackManager.GetCallBack()
        {
            return __CallBackDic.TryGetValue(__CallBackKey, out ICallBack callBack) ? callBack : null;
        }
    }
}
