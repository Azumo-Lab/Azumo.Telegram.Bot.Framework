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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    /// <summary>
    /// 进行框架运行所必须依赖的设置工作
    /// </summary>
    internal class TelegramBasic : ITelegramPartCreator
    {
        /// <summary>
        /// 创建时服务
        /// </summary>
        /// <param name="services"></param>
        public void AddBuildService(IServiceCollection services)
        {

        }

        /// <summary>
        /// 运行时服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

            ReflectionHelper.AllTypes.Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute)))
                .Select(x => (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, typeof(DependencyInjectionAttribute))))
                .ToList()
                .ForEach((x) =>
                {
                    switch (x.Item2.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = services.AddSingleton(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = services.AddScoped(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Transient:
                            _ = services.AddTransient(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        default:
                            break;
                    }
                });

            InternalInstall.StartInstall();

            services.AddLogging(option =>
            {
                option.AddConsole();
                option.AddSimpleConsole();
            });
            services.AddSingleton<ITelegramBot, TelegramBot>();
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加基础的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static ITelegramBotBuilder AddBasic(this ITelegramBotBuilder builder)
        {
            return builder.AddTelegramPartCreator(new TelegramBasic());
        }
    }
}
