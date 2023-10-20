//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    /// <summary>
    /// 进行框架运行所必须依赖的设置工作
    /// </summary>
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
            services.ScanTGService();

            InternalInstall.StartInstall();

            // 添加Log
            services.AddLogging(option =>
            {
                option.AddConsole();
                option.AddSimpleConsole();
            });
            // 添加 ITelegramBot
            services.AddSingleton<ITelegramBot, TelegramBot>();
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加基础的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static ITelegramBotBuilder AddBasic(this ITelegramBotBuilder builder)
        {
            return builder.AddTelegramPartCreator(new TelegramBasic());
        }
    }
}
