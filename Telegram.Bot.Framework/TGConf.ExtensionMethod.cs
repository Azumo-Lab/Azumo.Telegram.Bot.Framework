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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.MiddlewarePipelines;
using Telegram.Bot.Framework.ExtensionMethods;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 框架配置的一些扩展方法
    /// </summary>
    internal static class TGConf_ExtensionMethod
    {
        public static void AddMiddlewarePipeline(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewarePipeline>(serviceDescriptors);
        }

        public static void AddMiddlewareTemplate(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewareTemplate>(serviceDescriptors);
        }

        /// <summary>
        /// 程序会寻找<see cref="DependencyInjectionAttribute"/>标签，并将标注这个标签的对象注册到<see cref="IServiceCollection"/>中
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// 123
        /// </para>
        /// <para>
        /// 321
        /// </para>
        /// </remarks>
        /// <param name="serviceDescriptors">服务详细的列表</param>
        /// <exception cref="NotSupportedException"></exception>
        public static void AddDependencyInjection(this IServiceCollection serviceDescriptors)
        {
            Type objType = typeof(object);
            Type diAttr = typeof(DependencyInjectionAttribute);
            Reflection_ExtensionMethod.GetAllTypes()
                .Where(x => Attribute.IsDefined(x, diAttr))
                .ToList()
                .Select(x =>
                {
                    return (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, diAttr));
                })
                .OrderBy(x => x.Item2.Priority)
                .Select(x => x.x)
                .ToList()
                .ForEach(x =>
                {
                    if (Attribute.GetCustomAttribute(x, diAttr) is not DependencyInjectionAttribute dependencyInjectionAttribute)
                        return;

                    Type baseType;
                    Type[] interFaceType;
                    Type serviceType = dependencyInjectionAttribute.ServiceType ??
                            (((baseType = x.BaseType).FullName == objType.FullName)
                            ? ((interFaceType = x.GetInterfaces()).Length > 1
                                ? throw new NotSupportedException($"在 {x.FullName} 中，检测到多个接口类型：{string.Join(',', interFaceType.Select(x=>x.FullName).ToList())}")
                                : interFaceType.Length == 0 ? x : interFaceType[0])
                            : baseType);
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            serviceDescriptors.AddSingleton(serviceType, x);
                            break;
                        case ServiceLifetime.Scoped:
                            serviceDescriptors.AddScoped(serviceType, x);
                            break;
                        case ServiceLifetime.Transient:
                            serviceDescriptors.AddTransient(serviceType, x);
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
            foreach (Type item in baseType.GetSameTypes())
            {
                action.Invoke(serviceDescriptors, baseType, item);
            }
        }

        #endregion
    }
}
