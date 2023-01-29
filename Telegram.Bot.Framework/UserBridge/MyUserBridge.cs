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
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.Components;

namespace Telegram.Bot.Framework.UserBridge
{
    /// <summary>
    /// 用户桥的一个实现
    /// </summary>
    public class MyUserBridge : IUserBridge
    {
        /// <summary>
        /// 链接是否已经关闭
        /// </summary>
        public bool IsDiscard { get; private set; }

        /// <summary>
        /// 我自己
        /// </summary>
        public TelegramUser Me { get; private set; }

        /// <summary>
        /// 链接的目标用户
        /// </summary>
        public TelegramUser TargetUser { get; private set; }

        private readonly IServiceProvider serviceProvider;

        private readonly IUserScopeManager UserScopeManager;

        public event IUserBridge.OnCreateHandle OnCreate;
        public event IUserBridge.OnCloseHandle OnClose;

        #region 私有方法
        private TelegramContext GetTelegramContext(TelegramUser user)
        {
            IUserScope userScope = UserScopeManager.GetUserScope(user);
            return userScope.GetTelegramContext();
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Me">链接源用户</param>
        /// <param name="TargetUser">链接目标用户</param>
        /// <param name="serviceProvider">服务</param>
        public MyUserBridge(TelegramUser Me, TelegramUser TargetUser, IServiceProvider serviceProvider)
        {
            this.Me = Me;
            this.TargetUser = TargetUser;

            this.serviceProvider = serviceProvider;
            UserScopeManager = this.serviceProvider.GetService<IUserScopeManager>();
        }

        /// <summary>
        /// 销毁链接
        /// </summary>
        public void Dispose()
        {
            //设定关闭
            IsDiscard = true;

            //清除用户数据
            TargetUser = null;
            Me = null;
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <exception cref="Exception"></exception>
        public virtual async void Connect()
        {
            if (IsDiscard)
                throw new Exception("已经销毁链接，无法继续连接");

            OnCreate?.Invoke();

            TelegramContext MyContext = GetTelegramContext(Me);
            TelegramContext TargetUserContext = GetTelegramContext(TargetUser);

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

        /// <summary>
        /// 关闭链接
        /// </summary>
        public async void Disconnect()
        {
            OnClose?.Invoke();

            TelegramContext MyContext = GetTelegramContext(Me);
            TelegramContext TargetUserContext = GetTelegramContext(TargetUser);

            await MyContext.SendTextMessage("关闭连接...");
            await TargetUserContext.SendTextMessage("关闭连接...");
        }

        /// <summary>
        /// 发送一条文本消息
        /// </summary>
        /// <param name="Message">文本消息</param>
        public async void Send(string Message)
        {
            TelegramContext TargetUserContext = GetTelegramContext(TargetUser);

            await TargetUserContext.SendTextMessage(Message);
        }
    }
}
