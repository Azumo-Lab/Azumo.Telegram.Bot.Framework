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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Users;
using Telegram.Bot.Framework.Components;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.UserBridge
{
    /// <summary>
    /// TODO:用户桥功能暂时未实现
    /// </summary>
    internal class MyUserBridgeVer2 : IUserBridge
    {
        public string BridgeID { get; private set; }

        public bool IsDiscard { get; private set; }

        public TelegramUser TargetUser { get; private set; }

        public event IUserBridge.OnCreateHandle OnCreate;
        public event IUserBridge.OnCloseHandle OnClose;

        private readonly IServiceProvider ServiceProvider;

        public MyUserBridgeVer2(TelegramUser TargetUser, IServiceProvider ServiceProvider)
        {
            BridgeID = Guid.NewGuid().ToString();

            this.TargetUser = TargetUser;
            this.ServiceProvider = ServiceProvider;
        }

        public async Task Connect()
        {
            // 获取本人
            TelegramUser Me = ServiceProvider.GetTelegramContext().TelegramUser;
            Action<TelegramContext> CallBackActionOK = (Context) =>
            {

            };
            Action<TelegramContext> CallBackActionNO = async (Context) =>
            {
                await Context.SendTextMessage("你已经拒绝");
            };
            IUserScopeManager userScopeManager = ServiceProvider.GetService<IUserScopeManager>();

            IServiceProvider MyServiceProvider = ServiceProvider;
            IServiceProvider TargetServiceProvider = userScopeManager.GetUserScope(TargetUser).GetUserServiceScope().ServiceProvider;

            // 注册CallBack函数
            TelegramContext targetContext = TargetServiceProvider.GetTelegramContext();
            TelegramContext myContext = MyServiceProvider.GetTelegramContext();

            await targetContext.SendTextMessage($"用户 {Me.Username} 想要与你建立连接，是否同意", new List<InlineButtons> 
            { 
                InlineButtons.WithCallback("通过！", CallBackActionOK),
                InlineButtons.WithCallback("拒绝！", CallBackActionNO)
            });
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Send(string Message)
        {
            throw new NotImplementedException();
        }

        public Task Send(PhotoSize photo)
        {
            throw new NotImplementedException();
        }
    }
}
