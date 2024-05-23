//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
//
//  Author: 牛奶

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection ScanService(this IServiceCollection services)
        {
            var dependencyInjectionType = typeof(DependencyInjectionAttribute);
            foreach (var item in AllTypes)
            {
                if (Attribute.IsDefined(item, dependencyInjectionType))
                {
                    Attribute.GetCustomAttributes(item, dependencyInjectionType).Select(dependencyInjectionTypeLinq => (DependencyInjectionAttribute)dependencyInjectionTypeLinq).ToList().ForEach(dependencyInjection =>
                    {
                        var type = dependencyInjection.ServiceType;
                        if (type == null)
                        {
                            var interfaces = item.GetInterfaces();
                            if (interfaces.Length == 0)
                                type = item.BaseType ?? item;
                            else if (interfaces.Length == 1)
                            {
                                type = interfaces.First();
                                if (item.BaseType != null && item.BaseType.IsAbstract)
                                    throw new Exception($"类型：{item.FullName} 在 {item.BaseType.FullName} 和 {type.FullName} 之间不明确");
                            }
                            else throw new Exception($"发现复数的接口，请明确指定");
                        }

                        var key = dependencyInjection.Key;

#if NET6_0_OR_GREATER
                        _ = string.IsNullOrEmpty(key)
                           ? dependencyInjection.Lifetime switch
                           {
                               ServiceLifetime.Singleton => services.AddSingleton(type, item),
                               ServiceLifetime.Scoped => services.AddScoped(type, item),
                               ServiceLifetime.Transient => services.AddTransient(type, item),
                               _ => throw new Exception(),
                           }
                           : dependencyInjection.Lifetime switch
                           {
                               ServiceLifetime.Singleton => services.AddKeyedSingleton(type, key, item),
                               ServiceLifetime.Scoped => services.AddKeyedScoped(type, key, item),
                               ServiceLifetime.Transient => services.AddKeyedTransient(type, key, item),
                               _ => throw new Exception(),
                           };
#else
                        if (string.IsNullOrEmpty(key))
                            switch (dependencyInjection.Lifetime)
                            {
                                case ServiceLifetime.Singleton:
                                    services.AddSingleton(type, item);
                                    break;
                                case ServiceLifetime.Scoped:
                                    services.AddScoped(type, item);
                                    break;
                                case ServiceLifetime.Transient:
                                    services.AddTransient(type, item);
                                    break;
                                default:
                                    throw new Exception();
                            }
                        else
                            switch (dependencyInjection.Lifetime)
                            {
                                case ServiceLifetime.Singleton:
                                    services.AddKeyedSingleton(type, key, item);
                                    break;
                                case ServiceLifetime.Scoped:
                                    services.AddKeyedScoped(type, key, item);
                                    break;
                                case ServiceLifetime.Transient:
                                    services.AddKeyedTransient(type, key, item);
                                    break;
                                default:
                                    throw new Exception();
                            }
#endif
                    });
                }
            }
            return services;
        }
    }
}
