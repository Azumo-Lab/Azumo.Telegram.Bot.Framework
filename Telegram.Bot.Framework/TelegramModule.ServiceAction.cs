//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using Telegram.Bot.Framework.BotBuilder;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramServiceAction : ITelegramModule
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Action<IServiceCollection> _action;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public TelegramServiceAction(Action<IServiceCollection> action) => _action = action;

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
            _action(services);
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
        /// <param name="action"></param>
        /// <returns></returns>
        public static ITelegramModuleBuilder AddServiceAction(this ITelegramModuleBuilder builder, Action<IServiceCollection> action) =>
            builder.AddModule(new TelegramServiceAction(action));
    }
}