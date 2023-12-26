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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 添加配置文件
    /// </summary>
    /// <param name="settingPath"></param>
    [DebuggerDisplay("配置文件：{__SettingPath}")]
    internal class TelegramConfiguration(string settingPath) : ITelegramPartCreator
    {
        /// <summary>
        /// 配置文件的路径
        /// </summary>
        private readonly string __SettingPath = settingPath;

        private Type _type = null!;
        public void SetModel<T>() => _type = typeof(T);

        public void AddBuildService(IServiceCollection services)
        {
            ArgumentException.ThrowIfNullOrEmpty(__SettingPath, "配置文件路径");
            if (!File.Exists(__SettingPath))
                throw new FileNotFoundException($"未找到配置文件路径 : \"{__SettingPath}\"");

            _ = services.RemoveAll<IConfiguration>();
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(__SettingPath).Build();
            _ = services.AddSingleton<IConfiguration>(configurationBuilder);
            _ = services.AddSingleton(_type, configurationBuilder.Get(_type));
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            var configuration = builderService.GetRequiredService<IConfiguration>();
            _ = services.AddSingleton(configuration);
            if (_type != null)
                _ = services.AddSingleton(_type, builderService.GetService(_type));
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <remarks>
        /// 添加配置文件，添加这个配置文件将会覆盖其他的配置文件的配置
        /// </remarks>
        /// <param name="builder"></param>
        /// <param name="appSettingPath"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder AddConfiguration(this ITelegramBotBuilder builder, string appSettingPath) => builder.AddTelegramPartCreator(new TelegramConfiguration(appSettingPath));

        public static ITelegramBotBuilder AddConfiguration<T>(this ITelegramBotBuilder builder, string appSettingPath)
        {
            TelegramConfiguration telegramConfiguration = new(appSettingPath);
            telegramConfiguration.SetModel<T>();
            return builder.AddTelegramPartCreator(telegramConfiguration);
        }

        public static ITelegramBotBuilder AddConfiguration<T>(this ITelegramBotBuilder builder)
        {
            TelegramConfiguration telegramConfiguration = new("appsetting.json");
            telegramConfiguration.SetModel<T>();
            return builder.AddTelegramPartCreator(telegramConfiguration);
        }

        public static ITelegramBotBuilder AddConfiguration(this ITelegramBotBuilder builder) => builder.AddTelegramPartCreator(new TelegramConfiguration("appsetting.json"));
    }
}
