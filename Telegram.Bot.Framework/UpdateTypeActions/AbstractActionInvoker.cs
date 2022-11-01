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
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.UpdateTypeActions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractActionInvoker : IActionInvoker
    {
        protected IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// ActionHandle
        /// </summary>
        protected readonly ActionHandle ActionHandle;
        private readonly List<Func<ActionHandle, ActionHandle>> ActionHandles;

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
            ServiceProvider = serviceProvider;
            ActionHandles = new List<Func<ActionHandle, ActionHandle>>();

            AddActionHandles(ServiceProvider);

            ActionHandle = contexct => Task.CompletedTask;
            foreach (Func<ActionHandle, ActionHandle> item in ActionHandles.Reverse<Func<ActionHandle, ActionHandle>>())
                ActionHandle = item(ActionHandle);
        }

        /// <summary>
        /// 添加ActionHandle
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        protected abstract void AddActionHandles(IServiceProvider serviceProvider);

        protected virtual void AddHandle(IAction action)
        {
            ActionHandles.Add(
                    handle =>
                    context => action.Invoke(context, handle));
        }

        public abstract Task Invoke(TelegramContext context);
    }
}
