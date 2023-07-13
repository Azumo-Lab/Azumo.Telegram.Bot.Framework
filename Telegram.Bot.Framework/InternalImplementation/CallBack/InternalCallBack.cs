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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Models;

namespace Telegram.Bot.Framework.InternalImplementation.CallBack
{
    /// <summary>
    /// 
    /// </summary>
    internal class InternalCallBack : ICallBack
    {

        private Func<ITelegramChat, Task> __Callback;

        public InternalCallBack(Func<ITelegramChat, Task> callback)
        {
            __Callback = callback;
        }

        public void Dispose()
        {
            __Callback = null;
        }

        public CallBackResult Invoke(ITelegramChat telegramChat)
        {
            CallBackResult callBackResult = new();
            try
            {
                callBackResult.Success = true;
                callBackResult.Result = __Callback(telegramChat);
            }
            catch (Exception ex)
            {
                callBackResult.Success = false;
                callBackResult.Exception = ex;
            }
            return callBackResult;
        }
    }
}
