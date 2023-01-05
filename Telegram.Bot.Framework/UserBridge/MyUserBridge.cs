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
using Telegram.Bot.Framework.Components;

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
        public TelegramUser Me { get; private set; }
        public TelegramUser TargetUser { get; private set; }

        private IServiceProvider serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramUser"></param>
        /// <param name="telegramContext"></param>
        public MyUserBridge(TelegramUser Me, TelegramUser TargetUser, IServiceProvider serviceProvider)
        {
            this.Me = Me;
            this.TargetUser = TargetUser;
            this.serviceProvider = serviceProvider;
        }

        public event IUserBridge.OnCreateHandle OnCreate;
        public event IUserBridge.OnCloseHandle OnClose;

        public void Dispose()
        {
            IsDiscard = true;
            TargetUser = null;
            Me = null;
            serviceProvider = null;
        }

        public virtual async void Connect()
        {
            if (IsDiscard)
                throw new Exception("已经销毁链接，无法继续连接");

            OnCreate?.Invoke();

            IUserScopeManager userScopeManager = serviceProvider.GetService<IUserScopeManager>();
            IUserScope MyUserScope = userScopeManager.GetUserScope(Me);
            IUserScope TargetUserScope = userScopeManager.GetUserScope(TargetUser);
            
            TelegramContext MyContext = MyUserScope.GetTelegramContext();
            TelegramContext TargetUserContext = TargetUserScope.GetTelegramContext();

            await MyContext.SendTextMessage("正在连接...");
            await TargetUserContext.SendTextMessage("用户想与您联系...", new List<InlineButtons>
            {
                InlineButtons.WithCallback("同意", async Context =>
                {
                    await MyContext.SendTextMessage("对方同意链接");
                }),
                InlineButtons.WithCallback("拒绝", async Context =>
                {
                    await MyContext.SendTextMessage("对方拒绝链接");
                }),
            });
        }

        public async void Disconnect()
        {
            OnClose?.Invoke();

            IUserScopeManager userScopeManager = serviceProvider.GetService<IUserScopeManager>();
            IUserScope MyUserScope = userScopeManager.GetUserScope(Me);
            IUserScope TargetUserScope = userScopeManager.GetUserScope(TargetUser);

            TelegramContext MyContext = MyUserScope.GetTelegramContext();
            TelegramContext TargetUserContext = TargetUserScope.GetTelegramContext();

            await MyContext.SendTextMessage("关闭连接...");
            await TargetUserContext.SendTextMessage("关闭连接...");
        }

        public async void Send(string Message)
        {
            IUserScopeManager userScopeManager = serviceProvider.GetService<IUserScopeManager>();
            IUserScope TargetUserScope = userScopeManager.GetUserScope(TargetUser);
            TelegramContext TargetUserContext = TargetUserScope.GetTelegramContext();

            await TargetUserContext.SendTextMessage(Message);
        }
    }
}
