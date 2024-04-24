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

using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.CorePipeline;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Controller.Install;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
internal class TelegramControllerBuilder : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
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
public static class TelegramControllerBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder UseController(this ITelegramModuleBuilder builder) =>
        builder
        .AddModule(new TelegramControllerBuilder())
        .AddModule(new ScanService())
        .AddModule(new ScanController());
}
