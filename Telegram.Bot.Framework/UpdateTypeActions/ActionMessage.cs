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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework;
using Telegram.Bot.Framework.UpdateTypeActions.Actions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// 用于处理Message类型
    /// </summary>
    public class ActionMessage : AbstractActionInvoker
    {
        public override UpdateType InvokeType => UpdateType.Message;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider">DI服务</param>
        public ActionMessage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// 添加要执行的Action
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected override void AddActionHandles(IServiceProvider serviceProvider)
        {
            // 简单的认证
            AddHandle<ActionAuthentication>();
            // 群组消息处理
            AddHandle<ActionGroupChannel>();
            // 执行前过滤
            AddHandle<ActionFilterBefore>();
            // 参数获取
            AddHandle<ActionParamCatch>();
            // 执行命令控制器
            AddHandle<ActionControllerInvoke>();
            // 执行后过滤
            AddHandle<ActionFilterAfter>();
        }

        /// <summary>
        /// 每次执行前的前置执行操作
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task InvokeAction(TelegramContext context)
        {
            return Task.CompletedTask;
        }
    }
}
