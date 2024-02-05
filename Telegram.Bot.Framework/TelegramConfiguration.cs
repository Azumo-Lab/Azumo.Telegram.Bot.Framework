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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework;

/// <summary>
/// 进行配置文件的配置造作
/// </summary>
/// <param name="path">配置文件的路径</param>
[DebuggerDisplay("配置路径：“{ConfigPath}”，存在与否：“{File.Exists(ConfigPath)}”")]
internal class TelegramConfiguration(string path) : ITelegramModule
{
    /// <summary>
    /// 配置文件的路径
    /// </summary>
    protected readonly string ConfigPath = path;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void AddBuildService(IServiceCollection services)
    {
        if (!File.Exists(ConfigPath))
            throw new FileNotFoundException(ConfigPath);

        var config = new ConfigurationBuilder().AddJsonFile(ConfigPath).Build();
        services.AddSingleton<IConfiguration>(config);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    public void Build(IServiceCollection services, IServiceProvider builderService) => 
        services.TryAddSingleton(builderService.GetRequiredService<IConfiguration>());
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="SettingModel"></typeparam>
/// <param name="path"></param>
[DebuggerDisplay("配置路径：“{ConfigPath}”，存在与否：“{File.Exists(ConfigPath)}”")]
internal class TelegramConfiguration<SettingModel>(string path) : ITelegramModule where SettingModel : class
{
    /// <summary>
    /// 配置文件的路径
    /// </summary>
    private readonly string ConfigPath = path;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void AddBuildService(IServiceCollection services)
    {
        if (!File.Exists(ConfigPath))
            throw new FileNotFoundException(ConfigPath);

        var config = new ConfigurationBuilder().AddJsonFile(ConfigPath).Build();
        var setting = config.Get<SettingModel>() ?? throw new NullReferenceException("");

        services.AddSingleton(setting);
        services.AddSingleton<IConfiguration>(config);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        services.TryAddSingleton(builderService.GetRequiredService<SettingModel>());
        services.TryAddSingleton(builderService.GetRequiredService<IConfiguration>());
    }
}

/// <summary>
/// 
/// </summary>
public static class TelegramConfigurationExtensions
{
    /// <summary>
    /// 添加配置文件
    /// </summary>
    /// <typeparam name="SettingModel"></typeparam>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder AddConfiguration<SettingModel>(this ITelegramModuleBuilder builder, string path) where SettingModel : class =>
        builder.AddModule(new TelegramConfiguration<SettingModel>(path));
}
