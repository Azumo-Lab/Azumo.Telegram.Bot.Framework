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
using System.ComponentModel;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.Install;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
/// <param name="func"></param>
internal class TelegramCommand(Delegate func) : ITelegramModule
{
    private readonly Delegate _func = func;

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
    /// <exception cref="NullReferenceException"></exception>
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        var manager = builderService.GetRequiredService<ICommandManager>();
        var attr = Attribute.GetCustomAttribute(_func.Method, typeof(BotCommandAttribute)) ?? throw new NullReferenceException();

        var exec = Factory.GetExecutorInstance(EnumCommandType.Func,
            _func,
            _func.Method.GetParameters().Select(x => x.GetParams()).ToList(),
            TypeDescriptor.GetAttributes(_func.Method));

        manager.AddExecutor(exec);
    }
}

/// <summary>
/// 
/// </summary>
public static class TelegramCommandExtensions
{
    /// <summary>
    /// 添加一个命令
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder AddCommand(this ITelegramModuleBuilder builder, Delegate func) =>
        builder.AddModule<TelegramCommand>(func);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="commandName"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder AddCommand(this ITelegramModuleBuilder builder, string commandName, Delegate func)
    {
        _ = TypeDescriptor.AddAttributes(func.Method, new BotCommandAttribute(commandName));
        return builder.AddCommand(func);
    }
}
