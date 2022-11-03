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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// 机器人被加入群组等事件发生时
    /// </summary>
    public class ActionMyChatMemberChange : AbstractActionInvoker
    {
        public delegate Task TelegramInvoke(TelegramContext telegramContext);

        public event TelegramInvoke OnInvited;  //被邀请
        public event TelegramInvoke OnKicked;   //被踢
        public event TelegramInvoke OnLeft;     //离开群组
        public event TelegramInvoke OnCreator;  //创建聊天
        public event TelegramInvoke OnBeAdmin;  //成为管理员

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ActionMyChatMemberChange(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override UpdateType InvokeType => UpdateType.MyChatMember;

        protected override async Task InvokeAction(TelegramContext context)
        {
            switch (context.Update.MyChatMember.NewChatMember.Status)
            {
                case ChatMemberStatus.Creator:
                    await OnCreator?.Invoke(context);
                    break;
                case ChatMemberStatus.Administrator:
                    await OnBeAdmin?.Invoke(context);
                    break;
                case ChatMemberStatus.Member:
                    await OnInvited?.Invoke(context);
                    break;
                case ChatMemberStatus.Left:
                    await OnLeft?.Invoke(context);
                    break;
                case ChatMemberStatus.Kicked:
                    await OnKicked?.Invoke(context);
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
