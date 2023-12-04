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

        public void AddBuildService(IServiceCollection services)
        {
            _ = services.RemoveAll<IConfiguration>();
            services.TryAddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile(__SettingPath).Build());
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.AddSingleton(builderService.GetRequiredService<IConfiguration>());
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
        public static ITelegramBotBuilder AddConfiguration(this ITelegramBotBuilder builder, string appSettingPath)
        {
            return builder.AddTelegramPartCreator(new TelegramConfiguration(appSettingPath));
        }
    }
}
