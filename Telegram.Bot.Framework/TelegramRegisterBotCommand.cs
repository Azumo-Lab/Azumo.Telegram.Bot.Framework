﻿//  <Telegram.Bot.Framework>
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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Execs;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramRegisterBotCommand : ITelegramModule
    {
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
            services.AddSingleton<IStartTask, RegisterBotCommandExec>();

        /// <summary>
        /// 
        /// </summary>
        private class RegisterBotCommandExec : IStartTask
        {
            private readonly IServiceProvider serviceProvider;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="serviceProvider"></param>
            public RegisterBotCommandExec(IServiceProvider serviceProvider) =>
                this.serviceProvider = serviceProvider;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public async Task ExecuteAsync(object? input, CancellationToken token)
            {
                var commandManager = serviceProvider.GetRequiredService<ICommandManager>();
                var botCommand = commandManager.GetExecutorList()
                    .SelectMany(x => x.Attributes)
                    .Where(x => x is BotCommandAttribute)
                    .Select(x => (BotCommandAttribute)x)
                    .Select(x => new BotCommand { Command = x.BotCommand, Description = x.Description })
                    .ToList();

                var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();
                await botClient.SetMyCommandsAsync(botCommand, cancellationToken: token);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class TelegramRegisterBotCommandExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ITelegramModuleBuilder RegisterBotCommand(this ITelegramModuleBuilder builder) =>
            builder.AddModule<TelegramRegisterBotCommand>();
    }
}
