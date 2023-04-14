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
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalImplementation.Params
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class AbstractParamMiddlewarePipeline : IParamMiddlewarePipeline
    {
        public List<object> Param { get; } = new List<object>();

        private ParamMiddlewareDelegate __NextHandle = (session, param, context) => Task.FromResult(true);

        private readonly IServiceProvider __ServiceProvider;

        private readonly List<Func<ParamMiddlewareDelegate, ParamMiddlewareDelegate>> __Handles = new();

        public AbstractParamMiddlewarePipeline(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider;

            AddMiddlewareHandles(__ServiceProvider);

            foreach (Func<ParamMiddlewareDelegate, ParamMiddlewareDelegate> Handle in __Handles.Reverse<Func<ParamMiddlewareDelegate, ParamMiddlewareDelegate>>())
                __NextHandle = Handle(__NextHandle);
        }

        /// <summary>
        /// 添加中间件，创建中间件流水线
        /// </summary>
        /// <param name="ServiceProvider">DI服务</param>
        protected virtual void AddMiddlewareHandles(IServiceProvider ServiceProvider)
        {

        }

        /// <summary>
        /// 创建指定的一个对象
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="serviceProvider">DI服务</param>
        /// <returns>指定对象的实例</returns>
        protected void AddMiddleware<T>() where T : IParamMiddleware
        {
            IParamMiddleware paramMiddleware = ActivatorUtilities.CreateInstance<T>(__ServiceProvider, Array.Empty<object>());

            __Handles.Add(handle => (session, paramManager, controllerContext) =>
            {
                return paramMiddleware.Execute(session, paramManager, controllerContext, handle);
            });
        }

        public async Task<bool> Execute(ITelegramSession session, IControllerContext controllerContext)
        {
            IParamManager paramManager = __ServiceProvider.GetService<IParamManager>();
            bool result = await __NextHandle.Invoke(session, paramManager, controllerContext);
            if (result)
                Param.AddRange(paramManager.GetParams());
            return result;
        }
    }
}
