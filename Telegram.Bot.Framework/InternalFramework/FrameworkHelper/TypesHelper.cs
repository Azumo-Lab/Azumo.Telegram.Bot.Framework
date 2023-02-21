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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.InternalFramework.FrameworkHelper
{
    /// <summary>
    /// 
    /// </summary>
    internal static class TypesHelper
    {
        private readonly static List<Type> AllTypes;
        private readonly static Dictionary<string, List<Type>> Dic_AllTypes = new();

        /// <summary>
        /// 
        /// </summary>
        static TypesHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> GetTypes<T>()
        {
            Type BaseType = typeof(T);
            if (!Dic_AllTypes.ContainsKey(BaseType.FullName))
                Dic_AllTypes.Add(BaseType.FullName, AllTypes.Where(x => BaseType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface).ToList());
            return Dic_AllTypes[BaseType.FullName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetAttr<T>(MemberInfo info) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(info, typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetAttr<T>(ParameterInfo info) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(info, typeof(T));

        /// <summary>
        /// 将基于类型 T 的所有类型全部添加到 IServiceCollection 中
        /// </summary>
        /// <typeparam name="T">抽象类 / 接口</typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <param name="serviceLifetime"></param>
        /// <exception cref="ArgumentException">T 不是 抽象类 或 接口</exception>
        public static void AddServiceTypes<T>(this IServiceCollection serviceDescriptors, ServiceLifetime serviceLifetime) where T : class
        {
            Type baseType = typeof(T);
            if (!baseType.IsAbstract && !baseType.IsInterface)
                throw new ArgumentException($"类型 {baseType.FullName} 不是抽象类或接口");

            Action<IServiceCollection, Type> action = serviceLifetime switch
            {
                ServiceLifetime.Singleton => (services, implType) => { services.AddSingleton(baseType, implType); },
                ServiceLifetime.Scoped => (services, implType) => { services.AddScoped(baseType, implType); },
                ServiceLifetime.Transient => (services, implType) => { services.AddTransient(baseType, implType); },
                _ => throw new ArgumentException($"参数：{nameof(serviceLifetime)} 错误，错误值：“ {serviceLifetime} ” "),
            };

            foreach (Type item in GetTypes<T>())
            {
                action(serviceDescriptors, item);
            }
        }
    }
}
