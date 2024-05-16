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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.BotBuilder;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.InternalCore.CorePipelines.ControllerInvokePipeline;
using Telegram.Bot.Framework.InternalCore.CorePipelines.Models;
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
            services.AddScoped(x => PipelineFactory.GetPipelineBuilder<PipelineModel, Task>(() => Task.CompletedTask)
            .Use(new PipelineCommandScope())
            .Use(new PipelineGetParam())
            .Use(new PipelineControllerInvoke())
            .CreatePipeline(UpdateType.Message)
            .Use(new PipelineCallBack())
            .CreatePipeline(UpdateType.CallbackQuery)
            .Use(new PipelineMyChatMember())
            .CreatePipeline(UpdateType.MyChatMember)
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
        private readonly IServiceCollection ScopeServices = new ServiceCollection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void AddBuildService(IServiceCollection services)
        {
            _ = ScopeServices.AddSingleton<ICommandManager, CommandManager>();

            var serviceProvider = ScopeServices.BuildServiceProvider();

            // 获取参数实现类
            var getparamTypeList = typeof(IGetParam).GetAllSameType()
                .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
                .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
                .ToList();

            var commandManager = serviceProvider.GetRequiredService<ICommandManager>();

            var controllerTypeList = typeof(TelegramControllerAttribute).GetTypesWithAttribute();
            foreach ((var controller, _) in controllerTypeList)
                foreach ((var method, var attr) in controller.GetMethodsWithAttribute<BotCommandAttribute>(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                {
                    var attributes = new List<Attribute>
                    {
                        attr
                    };
                    attributes.AddRange(method.GetCustomAttributes());

                    var executor = Factory.GetExecutorInstance(EnumCommandType.BotCommand,
                        method.BuildFunc(),
                        method.GetParameters().Select(x => x.GetParams()).ToList(),
                        attributes.ToArray());
                    commandManager.AddExecutor(executor);
                }

            _ = services.AddSingleton(commandManager);
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
        public override Task<object> GetParam(TelegramContext context) =>
            Task.FromResult<object>(null!);
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
