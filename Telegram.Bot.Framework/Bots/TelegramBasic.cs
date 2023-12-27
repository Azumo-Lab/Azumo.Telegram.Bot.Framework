//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Reflection;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.InternalInterface;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 进行框架运行所必须依赖的设置工作
    /// </summary>
    /// <remarks>
    /// 进行基础服务的设置和处理
    /// </remarks>
    [DebuggerDisplay("框架基础服务")]
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
            var _LogActions = builderService.GetServices<Action<ILoggingBuilder, IServiceProvider>>();
            // 添加Log
            _ = services.AddLogging(option =>
            {
                if (_LogActions == null || !_LogActions.Any())
                    _ = option.AddSimpleConsole();
                else
                    foreach (var item in _LogActions)
                        item.Invoke(option, builderService);
            });
            // 添加 ITelegramBot
            _ = services.AddSingleton<ITelegramBot, TelegramBot>();
        }
    }

    [DebuggerDisplay("基础框架安装搜索服务")]
    internal class TelegramInstall : ITelegramPartCreator
    {
        public TelegramInstall()
        {

        }
        public void AddBuildService(IServiceCollection services)
        {
            _ = services.AddSingleton<IControllerParamMaker, ControllerParamMaker>();
            var reflection = AzReflection<ITelegramService>.Create();
            foreach (var item in reflection.FindAllSubclass())
            {
                _ = services.AddSingleton(typeof(ITelegramService), item);
            }
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.ScanService();

            services.AddSingleton<IControllerManager>(service =>
            {
                ControllerManager controllerManager = new();
                
                var azReflection = AzReflection<TelegramController>.Create();

                foreach (var controller in azReflection.FindAllSubclass())
                {
                    var methodinfos = controller.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var method in methodinfos)
                        if (Attribute.IsDefined(method, typeof(BotCommandAttribute)))
                            controllerManager.InternalCommands.Add(new BotCommand(builderService)
                            {
                                MethodInfo = method,
                            });
                }
                foreach (var command in builderService.GetServices<Delegate>() ?? [])
                {
                    controllerManager.InternalCommands.Add(new BotCommand(builderService)
                    {
                        MethodInfo = command.Method,
                        Target = command.Target,
                    });
                }
                controllerManager.InternalCommands.ForEach(x => x.Cache());
                return controllerManager;
            });

            foreach (var service in builderService.GetServices<ITelegramService>())
                service.AddServices(services);
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加基础的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static ITelegramBotBuilder AddBasic(this ITelegramBotBuilder builder) => builder
                .AddTelegramPartCreator(new TelegramInstall())
                .AddTelegramPartCreator(new TelegramBasic());
    }
}
