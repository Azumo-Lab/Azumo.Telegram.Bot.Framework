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

namespace Telegram.Bot.Framework.MiddlewarePipelines.Pipeline
{
    /// <summary>
    /// 流水线创建
    /// </summary>
    public class PipelineBuilder : IPipelineBuilder
    {
        private bool IsBuild;
        private MiddlewareDelegate __MiddlewareHandle = contexct => Task.CompletedTask;
        private readonly List<IMiddleware> __Middlewares = new();
        private readonly List<Func<MiddlewareDelegate, MiddlewareDelegate>> __MiddlewareHandles = new();
        private readonly IServiceProvider __ServiceProvider;

        public PipelineBuilder(IServiceProvider ServiceProvider)
        {
            __ServiceProvider = ServiceProvider;
        }

        /// <summary>
        /// 添加中间件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPipelineBuilder AddMiddleware<T>() where T : IMiddleware
        {
            __Middlewares.Add(CreateInstance<T>());
            return this;
        }

        /// <summary>
        /// 创建流水线委托
        /// </summary>
        /// <param name="PipelineController">流水线控制器</param>
        /// <returns></returns>
        public MiddlewareDelegate Builder(IPipelineController PipelineController)
        {
            if (IsBuild)
                return __MiddlewareHandle;

            foreach (IMiddleware Middleware in __Middlewares)
                __MiddlewareHandles.Add(Handle => Session =>
                {
                    PipelineController.SetNextHandle(Handle);
                    return Middleware.Execute(Session, PipelineController);
                });

            foreach (Func<MiddlewareDelegate, MiddlewareDelegate> Handle in __MiddlewareHandles.Reverse<Func<MiddlewareDelegate, MiddlewareDelegate>>())
                __MiddlewareHandle = Handle(__MiddlewareHandle);

            IsBuild = true;

            return __MiddlewareHandle;
        }

        /// <summary>
        /// 创建一个对象的实例
        /// </summary>
        /// <typeparam name="T">需要创建的对象</typeparam>
        /// <returns>实例</returns>
        private T CreateInstance<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(__ServiceProvider, Array.Empty<object>());
        }
    }
}
