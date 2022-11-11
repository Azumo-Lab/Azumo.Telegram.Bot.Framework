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
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// 机器人被加入群组等事件发生时
    /// </summary>
    public class ActionMyChatMemberChange : AbstractActionInvoker
    {
        private readonly IBotTelegramEvent telegramEvent;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ActionMyChatMemberChange(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
             telegramEvent = serviceProvider.GetService<IBotTelegramEvent>();
        }

        public override UpdateType InvokeType => UpdateType.MyChatMember;

        protected override async Task InvokeAction(TelegramContext context)
        {
            switch (context.Update.MyChatMember.NewChatMember.Status)
            {
                case ChatMemberStatus.Creator://创建聊天
                    await telegramEvent?.OnCreator(context);
                    break;
                case ChatMemberStatus.Administrator://成为管理员
                    await telegramEvent?.OnBeAdmin(context);
                    break;
                case ChatMemberStatus.Member://被邀请
                    await telegramEvent?.OnInvited(context);
                    break;
                case ChatMemberStatus.Left://离开
                    await telegramEvent?.OnLeft(context);
                    break;
                case ChatMemberStatus.Kicked://被踢
                    await telegramEvent?.OnKicked(context);
                    break;
                case ChatMemberStatus.Restricted:
                    break;
                default:
                    break;
            }
        }

        protected override void AddActionHandles(IServiceProvider serviceProvider)
        {
            
        }
    }
}
