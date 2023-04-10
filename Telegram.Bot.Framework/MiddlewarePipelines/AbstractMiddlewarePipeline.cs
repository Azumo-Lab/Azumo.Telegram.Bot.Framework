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
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 中间件流水线的抽象执行类
    /// </summary>
    public abstract class AbstractMiddlewarePipeline : IMiddlewarePipeline
    {
        /// <summary>
        /// 中间件Handle
        /// </summary>
        private readonly MiddlewareHandle MiddlewareHandle = contexct => Task.CompletedTask;
        private readonly List<Func<MiddlewareHandle, MiddlewareHandle>> MiddlewareHandles = new();
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// 执行的类型
        /// </summary>
        public abstract UpdateType InvokeType { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        public AbstractMiddlewarePipeline(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;
            AddMiddlewareHandleTemplate(ServiceProvider);

            foreach (Func<MiddlewareHandle, MiddlewareHandle> Handle in MiddlewareHandles.Reverse<Func<MiddlewareHandle, MiddlewareHandle>>())
                MiddlewareHandle = Handle(MiddlewareHandle);
        }

        /// <summary>
        /// 添加一个模板方法
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        private void AddMiddlewareHandleTemplate(IServiceProvider ServiceProvider)
        {
            List<IMiddlewareTemplate> middlewareTemplates = ServiceProvider.GetServices<IMiddlewareTemplate>()?.ToList() ?? new List<IMiddlewareTemplate>();

            middlewareTemplates.ForEach(x => x.BeforeAddMiddlewareHandles(ServiceProvider));

            AddMiddlewareHandles(ServiceProvider);

            middlewareTemplates.ForEach(x => x.AfterAddMiddlewareHandles(ServiceProvider));
        }

        /// <summary>
        /// 添加中间件，创建中间件流水线
        /// </summary>
        /// <param name="ServiceProvider">DI服务</param>
        protected virtual void AddMiddlewareHandles(IServiceProvider ServiceProvider) 
        { 
            
        }

        /// <summary>
        /// 用于向流水线中添加中间件
        /// </summary>
        protected virtual void AddMiddleware<T>() where T : IMiddleware
        {
            IMiddleware Middleware = CreateInstance<T>();
            MiddlewareHandles.Add(Handle => Session => Middleware.Execute(Session, Handle));
        }

        /// <summary>
        /// 执行这个流水线
        /// </summary>
        /// <param name="context">访问的请求对话</param>
        /// <returns>异步可等待的Task</returns>
        public async Task Execute(ITelegramSession Session)
        {
            await InvokeAction(Session);
            await MiddlewareHandle.Invoke(Session);
        }

        /// <summary>
        /// 每次执行前的前置执行操作
        /// </summary>
        /// <param name="context">访问的请求对话</param>
        /// <returns>无</returns>
        protected virtual async Task InvokeAction(ITelegramSession Session)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 创建指定的一个对象
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="serviceProvider">DI服务</param>
        /// <returns>指定对象的实例</returns>
        protected virtual T CreateInstance<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(ServiceProvider, Array.Empty<object>());
        }
    }
}
