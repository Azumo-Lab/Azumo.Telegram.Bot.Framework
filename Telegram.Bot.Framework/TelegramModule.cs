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
//
//  Author: 牛奶

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using Telegram.Bot.Framework.BotBuilder;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 配置文件扩展方法
    /// </summary>
    public static partial class TelegramModuleExtensions
    {
        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <remarks>
        /// 向框架中添加配置文件，配置文件的类型为 <typeparamref name="SettingModel"/>。<br/>
        /// 当添加配置文件后，可以使用 services.GetRequiredService{<typeparamref name="SettingModel"/>}() 来获取配置文件的实例。<br/>
        /// </remarks>
        /// <typeparam name="SettingModel">配置文件的类型</typeparam>
        /// <param name="builder">创建器</param>
        /// <param name="path">配置文件的路径位置</param>
        /// <returns>创建器</returns>
        public static ITelegramModuleBuilder AddConfiguration<SettingModel>(this ITelegramModuleBuilder builder, string path) where SettingModel : class =>
            builder.AddModule(new TelegramConfiguration<SettingModel>(path));
    }

    /// <summary>
    /// 进行配置文件的配置造作
    /// </summary>
    [DebuggerDisplay("配置路径：“{ConfigPath}”，存在与否：“{File.Exists(ConfigPath)}”")]
    internal class TelegramConfiguration : ITelegramModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">配置文件的路径</param>
        public TelegramConfiguration(string path) => ConfigPath = path;

        /// <summary>
        /// 配置文件的路径
        /// </summary>
        protected readonly string ConfigPath;

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
            _ = services.AddSingleton<IConfiguration>(config);
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
    [DebuggerDisplay("配置路径：“{ConfigPath}”，存在与否：“{File.Exists(ConfigPath)}”")]
    internal class TelegramConfiguration<SettingModel> : ITelegramModule where SettingModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">配置文件的路径</param>
        public TelegramConfiguration(string path) => ConfigPath = path;

        /// <summary>
        /// 配置文件的路径
        /// </summary>
        private readonly string ConfigPath;

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
            var setting = config.Get<SettingModel>()
                ?? throw new Exception($"无法将配置文件的数据赋值给 {typeof(SettingModel)}, 请检查配置文件是否正确");

            _ = services.AddSingleton(setting);
            _ = services.AddSingleton<IConfiguration>(config);
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
}
