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
using System.Linq;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 一个扩展类
    /// </summary>
    internal static class IServiceCollection_ExtensionMethod
    {
        /// <summary>
        /// 使用 <see cref="DependencyInjectionAttribute"/> 标签
        /// </summary>
        /// <remarks>
        /// 程序会寻找 <see cref="DependencyInjectionAttribute"/> 标签，并将指定的类和接口注册<br></br>
        /// 如果一个类型实现了多个接口，但是没有指定注册的类型，则会抛出 <see cref="NotSupportedException"/> 异常
        /// </remarks>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">未提供支持</exception>
        public static IServiceCollection UseDependencyInjectionAttribute(this IServiceCollection serviceDescriptors)
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
                                ? throw new NotSupportedException($"在 {x.FullName} 中，检测到多个接口类型：{string.Join(',', interFaceType.Select(x => x.FullName).ToList())}")
                                : interFaceType.Length == 0 ? x : interFaceType[0])
                            : baseType);
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = serviceDescriptors.AddSingleton(serviceType, x);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = serviceDescriptors.AddScoped(serviceType, x);
                            break;
                        case ServiceLifetime.Transient:
                            _ = serviceDescriptors.AddTransient(serviceType, x);
                            break;
                        default:
                            break;
                    }
                });
            return serviceDescriptors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection UseCompileDelegate(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors;
        }

        #region 将IServiceCollection 中的添加服务的方法扩展了

        /// <summary>
        /// 注册所有指定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceDescriptors"></param>
        internal static void RegisterSingletonServiceTypeOf<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                _ = serviceDescriptors.AddSingleton(baseType, implType);
            });
        }

        /// <summary>
        /// 注册所有指定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceDescriptors"></param>
        internal static void RegisterScopedServiceTypeOf<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                _ = serviceDescriptors.AddScoped(baseType, implType);
            });
        }

        /// <summary>
        /// 注册所有指定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceDescriptors"></param>
        internal static void RegisterTransientServiceTypeOf<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                _ = serviceDescriptors.AddTransient(baseType, implType);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <param name="action"></param>
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
