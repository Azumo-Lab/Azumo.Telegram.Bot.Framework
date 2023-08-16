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

using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Services;
using Telegram.Bot.Framework.Abstracts.User;

namespace Telegram.Bot.Framework.InternalProc.Services
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection<ICallbackService>(ServiceLifetime.Scoped)]
    internal class CallbackService : ICallbackService
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly Dictionary<string, Func<IChat, Task>> __CallBack = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callbackfunc"></param>
        /// <returns></returns>
        public string CreateCallback(Func<IChat, Task> callbackfunc)
        {
            string result = Guid.NewGuid().ToString();
            __CallBack.Add(result, callbackfunc);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            __CallBack.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public async Task InvokeCallback(IChat chat)
        {
            string key;
            if (!string.IsNullOrEmpty(key = chat.GetCallbackKey()))
                if (__CallBack.TryGetValue(key, out Func<IChat, Task> invokeFunc))
                    await invokeFunc(chat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callbackData"></param>
        public void RemoveCallback(string callbackData)
        {
            __CallBack.Remove(callbackData);
        }
    }
}
