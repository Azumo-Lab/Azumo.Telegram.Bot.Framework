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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Logger;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.MiddlewarePipelines;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.InternalImplementation.Params;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Framework.MiddlewarePipelines.Pipeline;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 框架配置
    /// </summary>
    internal class FrameworkConfig : IConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISession, InternalSession>();
            services.AddScoped<IParamManager, MyParamManager>();

            #region 中间件流水线相关的处理
            // 添加中间件流水线
            services.AddMiddlewarePipeline();
            services.AddMiddlewareTemplate();
            services.AddTransient<IPipelineBuilder, PipelineBuilder>();
            services.AddTransient<IPipelineController, PipelineController>();
            #endregion
        }
    }

    /// <summary>
    /// 框架配置的一些扩展方法
    /// </summary>
    internal static class FrameworkConfig_ExtensionMethod
    {
        public static void AddMiddlewarePipeline(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewarePipeline>(serviceDescriptors);
        }

        public static void AddMiddlewareTemplate(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewareTemplate>(serviceDescriptors);
        }

        public static void AddDependencyInjection(this IServiceCollection serviceDescriptors)
        {
            Type diAttr = typeof(DependencyInjectionAttribute);
            ObjectHelper.GetAllTypes()
                .Where(x => Attribute.IsDefined(diAttr, x))
                .ToList()
                .ForEach(x =>
                {
                    if (Attribute.GetCustomAttribute(x, diAttr) is not DependencyInjectionAttribute dependencyInjectionAttribute)
                        return;
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            serviceDescriptors.AddSingleton(dependencyInjectionAttribute.InterfaceType ?? x.BaseType, x);
                            break;
                        case ServiceLifetime.Scoped:
                            serviceDescriptors.AddScoped(dependencyInjectionAttribute.InterfaceType ?? x.BaseType, x);
                            break;
                        case ServiceLifetime.Transient:
                            serviceDescriptors.AddTransient(dependencyInjectionAttribute.InterfaceType ?? x.BaseType, x);
                            break;
                        default:
                            break;
                    }
                });
        }

        #region 将IServiceCollection 中的添加服务的方法扩展了

        private static void AddSingleton<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddSingleton(baseType, implType);
            });
        }

        private static void AddScoped<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddScoped(baseType, implType);
            });
        }

        private static void AddTransient<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddTransient(baseType, implType);
            });
        }

        private static void AddTemplate<T>(IServiceCollection serviceDescriptors, Action<IServiceCollection, Type, Type> action)
        {
            Type baseType = typeof(T);
            foreach (Type item in ObjectHelper.GetSameType(baseType))
            {
                action.Invoke(serviceDescriptors, baseType, item);
            }
        }

        #endregion
    }
}
