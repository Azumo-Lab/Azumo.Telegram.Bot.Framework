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
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// Action执行
    /// </summary>
    public abstract class AbstractActionInvoker : IActionInvoker
    {
        /// <summary>
        /// ActionHandle
        /// </summary>
        private readonly ActionHandle ActionHandle;
        private readonly List<Func<ActionHandle, ActionHandle>> ActionHandles;

        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 执行的类型
        /// </summary>
        public abstract UpdateType InvokeType { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public AbstractActionInvoker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            ActionHandles = new List<Func<ActionHandle, ActionHandle>>();

            AddActionHandles(serviceProvider);

            ActionHandle = contexct => Task.CompletedTask;
            foreach (Func<ActionHandle, ActionHandle> item in ActionHandles.Reverse<Func<ActionHandle, ActionHandle>>())
                ActionHandle = item(ActionHandle);
        }

        /// <summary>
        /// 添加ActionHandle
        /// </summary>
        /// <param name="serviceProvider">DI服务</param>
        protected abstract void AddActionHandles(IServiceProvider serviceProvider);

        /// <summary>
        /// 添加Action
        /// </summary>
        /// <param name="action">Action</param>
        protected virtual void AddHandle<T>() where T : IAction
        {
            IAction action = CreateObj<T>();
            ActionHandles.Add(
                    handle =>
                    context => action.Invoke(context, handle));
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context">Context信息</param>
        /// <returns>无</returns>
        public async Task Invoke(TelegramContext context)
        {
            await InvokeAction(context);
            await ActionHandle.Invoke(context);
        }

        /// <summary>
        /// 执行ActionHandles之前执行
        /// </summary>
        /// <param name="context">Context信息</param>
        /// <returns>无</returns>
        protected abstract Task InvokeAction(TelegramContext context);

        /// <summary>
        /// 创建指定的一个对象
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="serviceProvider">DI服务</param>
        /// <returns>指定对象的实例</returns>
        protected virtual T CreateObj<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, Array.Empty<object>());
        }
    }
}
