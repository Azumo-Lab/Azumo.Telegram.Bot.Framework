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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Event;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.MiddlewarePipelines.Middlewares;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 机器人被加入群组等事件发生时
    /// </summary>
    internal class PipelineMyChatMemberChange : AbstractMiddlewarePipeline
    {
        private readonly IBotTelegramEvent telegramEvent;

        private delegate Task TelegramEventDelegate(ITelegramSession session);

        private event TelegramEventDelegate OnCreator;
        private event TelegramEventDelegate OnBeAdmin;
        private event TelegramEventDelegate OnInvited;
        private event TelegramEventDelegate OnLeft;
        private event TelegramEventDelegate OnKicked;
        private event TelegramEventDelegate OnRestricted;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PipelineMyChatMemberChange(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            telegramEvent = serviceProvider.GetService<IBotTelegramEvent>();
            if (telegramEvent != null)
            {
                OnCreator += telegramEvent.OnCreator;
                OnBeAdmin += telegramEvent.OnBeAdmin;
                OnInvited += telegramEvent.OnInvited;
                OnLeft += telegramEvent.OnLeft;
                OnKicked += telegramEvent.OnKicked;
                OnRestricted += telegramEvent.OnRestricted;
            }
        }

        public override UpdateType InvokeType => UpdateType.MyChatMember;

        protected override async Task InvokeAction(ITelegramSession session)
        {
            Task task = null;
            switch (session.Update.MyChatMember.NewChatMember.Status)
            {
                case ChatMemberStatus.Creator://创建聊天
                    task = OnCreator?.Invoke(session);
                    break;
                case ChatMemberStatus.Administrator://成为管理员
                    task = OnBeAdmin?.Invoke(session);
                    break;
                case ChatMemberStatus.Member://被邀请
                    task = OnInvited?.Invoke(session);
                    break;
                case ChatMemberStatus.Left://离开
                    task = OnLeft?.Invoke(session);
                    break;
                case ChatMemberStatus.Kicked://被踢
                    task = OnKicked?.Invoke(session);
                    break;
                case ChatMemberStatus.Restricted:
                    task = OnRestricted?.Invoke(session);
                    break;
            }
            if (task != null)
                await task;
        }

        protected override void AddMiddlewareHandles(IServiceProvider serviceProvider)
        {
        }
    }
}
