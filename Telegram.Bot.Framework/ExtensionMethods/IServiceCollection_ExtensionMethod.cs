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
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    internal static class IServiceCollection_ExtensionMethod
    {
        /// <summary>
        /// 使用 <see cref="DependencyInjectionAttribute"/> 标签
        /// </summary>
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
            return serviceDescriptors;
        }

        public static IServiceCollection UseCompileDelegate(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors;
        }
    }
}
