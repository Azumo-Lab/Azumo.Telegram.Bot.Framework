//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Framework.InternalFramework/>
//
//  This program is free software: you can redistribute it and/or modify
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public TelegramUserScope(IServiceProvider service)
        {
            UserScope ??= service.CreateScope();
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
            //创建单次访问的Scope
            TelegramRouteController controller = new TelegramRouteController(OneTimeScope, UserScope);

            await controller.StartProcess();
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
