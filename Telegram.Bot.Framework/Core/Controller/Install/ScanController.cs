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

using Azumo.SuperExtendedFramework;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.I18N;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Install;

/// <summary>
/// 
/// </summary>
internal class ScanController : ITelegramModule
{
    private readonly IServiceCollection ScopeServices = new ServiceCollection();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void AddBuildService(IServiceCollection services)
    {
        ScopeServices.AddSingleton<ICommandManager, CommandManager>();

        var serviceProvider = ScopeServices.BuildServiceProvider();

        // 获取参数实现类
        var getparamTypeList = typeof(IGetParam).GetAllSameType()
            .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
            .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
            .ToList();

        var commandManager = serviceProvider.GetRequiredService<ICommandManager>();

        var controllerTypeList = typeof(TelegramControllerAttribute).GetHasAttributeType();
        foreach ((var controller, var _) in controllerTypeList)
        {
            foreach ((var method, var attr) in controller.GetAttributeMethods<BotCommandAttribute>(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                var attributes = new List<Attribute>
                {
                    attr
                };
                attributes.AddRange(method.GetCustomAttributes());
                var executor = Factory.GetExecutorInstance(EnumCommandType.BotCommand,
                    ActivatorUtilities.CreateFactory(controller, []),
                    method.BuildFunc(),
                    method.GetParameters().Select(x => x.GetParams()).ToList(),
                    attributes.ToArray());
                commandManager.AddExecutor(executor);
            }
        }

        services.AddSingleton(commandManager);
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
internal class NullParam : IGetParam
{
    public ParamAttribute? ParamAttribute { get; set; }

    public Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(null!);
    public Task<bool> SendMessage(TelegramUserContext context) =>
        Task.FromResult(true);
}
