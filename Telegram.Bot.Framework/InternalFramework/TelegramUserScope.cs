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
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramUserScope : IDisposable, ITelegramUserScope
    {
        /// <summary>
        /// 
        /// </summary>
        private IServiceScope UserScope { get; }

        private readonly ActionHandle handle = async (x, y, z) => { await Task.Delay(1); };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public TelegramUserScope(IServiceProvider service)
        {
            UserScope ??= service.CreateScope();

            IEnumerable<IAction> actions = UserScope.ServiceProvider.GetServices<IAction>();
            List<Func<ActionHandle, ActionHandle>> ActionHandles = new();

            actions = actions.OrderByDescending(x => ((IHandleSort)x).Sort).ToList();

            foreach (IAction item in actions)
                ActionHandles.Add(
                    handle =>
                    (context, userscope, onetimescope) => item.Invoke(context, userscope, onetimescope, handle));

            foreach (Func<ActionHandle, ActionHandle> item in ActionHandles)
                handle = item(handle);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            UserScope.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OneTimeScope"></param>
        /// <returns></returns>
        public async Task Invoke(IServiceScope OneTimeScope)
        {
            TelegramContext context = OneTimeScope.ServiceProvider.GetService<TelegramContext>();
            await handle.Invoke(context, UserScope, OneTimeScope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IServiceScope GetUserScope()
        {
            return UserScope;
        }
    }
}
