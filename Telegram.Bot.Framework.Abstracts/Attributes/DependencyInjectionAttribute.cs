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

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 将指定的服务注册
    /// </summary>
    /// <remarks>
    /// 注册指定的服务，需要在需要注册的实现类上面使用本属性。
    /// </remarks>
    /// <typeparam name="T">要注册的服务类型</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DependencyInjectionAttribute : Attribute
    {
        /// <summary>
        /// 指定服务的生命周期
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        /// <summary>
        /// 自动赋值，服务的类型
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// 用于控制顺序
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceLifetime"></param>
        public DependencyInjectionAttribute(ServiceLifetime ServiceLifetime, Type ServiceType)
        {
            ServiceType.ThrowIfNull();

            this.ServiceLifetime = ServiceLifetime;
            this.ServiceType = ServiceType;
        }
    }

    /// <summary>
    /// 将指定服务注册标签的扩展方法
    /// </summary>
    public static class DependencyInjectionAttributeEX
    {
        /// <summary>
        /// 将标签的内容注册
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>注册之后的集合内容</returns>
        public static IServiceCollection UseDependencyInjectionAttribute(this IServiceCollection services)
        {
            List<Type> types = Helper.AllTypes.Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute))).ToList();

            List<(DependencyInjectionAttribute attr, Type x)> newtypes = types
                .Select(x =>
                {
                    DependencyInjectionAttribute attr = (DependencyInjectionAttribute)x.GetCustomAttribute(typeof(DependencyInjectionAttribute))!;
                    return (attr, x);
                })
                .OrderBy(x => x.attr.Priority)
                .ToList();

            foreach ((DependencyInjectionAttribute attr, Type x) in newtypes)
            {
                switch (attr.ServiceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(attr.ServiceType, x);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(attr.ServiceType, x);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(attr.ServiceType, x);
                        break;
                    default:
                        throw new ArgumentException($"{nameof(Type)}: {attr.ServiceType.FullName}, {nameof(ServiceLifetime)}: {attr.ServiceLifetime}");
                }
            }
            return services;
        }
    }
}
