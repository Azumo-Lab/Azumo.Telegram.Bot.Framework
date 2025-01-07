//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.BotBuilder;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Params;
using Telegram.Bot.Framework.CorePipelines;
using Telegram.Bot.Framework.InternalCore.Install;
using Telegram.Bot.Framework.PipelineMiddleware;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramPipelineBuilder : ITelegramModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void AddBuildService(IServiceCollection services)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        public void Build(IServiceCollection services, IServiceProvider builderService) =>
            services.AddScoped(x => PipelineFactory.GetPipelineBuilder<TelegramActionContext, Task>(() => Task.CompletedTask)
            .Use(new ControllerPipelineGetCommand())
            .Use(new ControllerPipelineControllerInvoker())
            .CreatePipeline(UpdateType.Message)
            .Use(new ControllerPipelineGetCommand())
            .Use(new ControllerPipelineControllerInvoker())
            .Use(new ControllerPipelineCallBackAnswer())
            .CreatePipeline(UpdateType.CallbackQuery)
            .Build());
    }

    /// <summary>
    /// 
    /// </summary>
    internal class TelegramScanService : ITelegramModule
    {
        public void AddBuildService(IServiceCollection services)
        {

        }
        public void Build(IServiceCollection services, IServiceProvider builderService) =>
            services.ScanService();
    }

    /// <summary>
    /// 
    /// </summary>
    internal class TelegramScanController : ITelegramModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void AddBuildService(IServiceCollection services)
        {
            var controllerType = typeof(TelegramController);
            var commandList = new List<IExecutor>();
            foreach (var type in Extensions.AllTypes)
            {
                if (!(Attribute.GetCustomAttribute(type, typeof(TelegramControllerAttribute)) != null
                    || (controllerType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)))
                    continue;

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var item in methods)
                {
                    if (!Attribute.IsDefined(item, typeof(BotCommandAttribute)))
                        continue;

                    var executor = Factory.GetExecutorInstance(EnumCommandType.BotCommand);
                    executor.Analyze(item);
                    commandList.Add(executor);
                }
            }
            services.AddSingleton(x =>
            {
                ICommandManager commandManager = new CommandManager();
                foreach (var item in commandList)
                    commandManager.AddExecutor(item);

                return commandManager;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        /// <exception cref="Exception"></exception>
        public void Build(IServiceCollection services, IServiceProvider builderService) =>
            services.AddSingleton(builderService.GetRequiredService<ICommandManager>());
    }

    /// <summary>
    /// 一个空的参数获取实现类
    /// </summary>
    internal class NullParam : BaseGetParamDirect
    {
        public override Task<object?> GetParam(TelegramActionContext context) =>
            Task.FromResult<object?>(null);
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class TelegramModuleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ITelegramModuleBuilder UseController(this ITelegramModuleBuilder builder) =>
            builder
            .AddModule(new TelegramPipelineBuilder())
            .AddModule(new TelegramScanService())
            .AddModule(new TelegramScanController());
    }
}
