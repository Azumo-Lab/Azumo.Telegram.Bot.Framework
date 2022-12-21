//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;

namespace Telegram.Bot.Framework.UserBridge
{
    /// <summary>
    /// 
    /// </summary>
    public class MyUserBridge : IUserBridge
    {
        public bool IsDiscard { get; private set; }

        /// <summary>
        /// 目标用户
        /// </summary>
        public TelegramUser TargetUser { get; private set; }
        public TelegramContext Context { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramUser"></param>
        /// <param name="telegramContext"></param>
        public MyUserBridge(TelegramUser telegramUser, TelegramContext telegramContext)
        {
            TargetUser = telegramUser;
            Context = telegramContext;
        }

        public event IUserBridge.OnCreateHandle OnCreate;
        public event IUserBridge.OnCloseHandle OnClose;

        public void Dispose()
        {
            OnClose?.Invoke();
            IsDiscard = true;
            TargetUser = null;
        }

        public virtual async void Connect()
        {
            OnCreate?.Invoke();

            IServiceProvider MyScope = Context.UserScope;
            IServiceProvider TargetScope = MyScope.GetService<IUserManager>().GetUserScope(TargetUser).ServiceProvider;

            ICallBackManager MyCallBack = MyScope.GetService<ICallBackManager>();
            ICallBackManager TargetCallBack = TargetScope.GetService<ICallBackManager>();

            MyCallBack.CreateCallBack(context =>
            {

            });
        }
    }
}
