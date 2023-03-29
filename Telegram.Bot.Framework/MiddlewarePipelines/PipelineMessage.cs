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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.MiddlewarePipelines.Middlewares;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 用于处理Message类型
    /// </summary>
    internal class PipelineMessage : AbstractMiddlewarePipeline
    {
        public override UpdateType InvokeType => UpdateType.Message;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider">DI服务</param>
        public PipelineMessage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// 添加要执行的Action
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected override void AddMiddlewareHandles(IServiceProvider serviceProvider)
        {
            // 简单的认证
            AddMiddleware<ActionAuthentication>();
            // 群组消息处理
            AddMiddleware<ActionGroupChannel>();
            // 执行前过滤
            AddMiddleware<ActionFilterBefore>();
            // 参数获取
            AddMiddleware<ActionParamCatch>();
            // 执行命令控制器
            AddMiddleware<ActionControllerInvoke>();
            // 执行后过滤
            AddMiddleware<ActionFilterAfter>();

            AddMiddleware<ActionUpdateTypeInvoke>();
        }

        /// <summary>
        /// 每次执行前的前置执行操作
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task InvokeAction(ITelegramSession session)
        {
            return Task.CompletedTask;
        }
    }
}
