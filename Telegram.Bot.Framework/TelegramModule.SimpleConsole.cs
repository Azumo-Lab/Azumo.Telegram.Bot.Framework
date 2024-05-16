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
using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot.Framework.BotBuilder;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramSimpleConsole : ITelegramModule
    {
        public void AddBuildService(IServiceCollection services)
        {

        }
        public void Build(IServiceCollection services, IServiceProvider builderService) =>
            services.AddLogging(x => x.AddSimpleConsole());
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
        public static ITelegramModuleBuilder AddSimpleConsole(this ITelegramModuleBuilder builder) =>
            builder.AddModule<TelegramSimpleConsole>();
    }
}
