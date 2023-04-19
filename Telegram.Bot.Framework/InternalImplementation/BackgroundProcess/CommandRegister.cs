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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.BackgroundProcess;
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.BackgroundProcess
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class CommandRegister : IStartBeforeExec
    {
        private readonly IServiceProvider serviceProvider;
        public CommandRegister(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 将目前的指令注册到TelegramBot中
        /// </summary>
        public async Task Exec()
        {
            ITelegramCommandsManager telegramCommandsManager = serviceProvider.GetService<ITelegramCommandsManager>();
            IControllerContextFactory controllerContextFactory = serviceProvider.GetService<IControllerContextFactory>();

            List<BotCommand> botCommands = controllerContextFactory.GetAllCommandControllerContext()
                .Where(x => x.BotCommandAttribute.Register)
                .Select(x => new BotCommand()
                {
                    Command = x.BotCommandAttribute.Command,
                    Description = x.BotCommandAttribute.Description,
                }).ToList();

            using (ITelegramCommandsChangeSession telegramCommandsChangeSession = telegramCommandsManager.ChangeTelegramCommands())
            {
                foreach (BotCommand item in botCommands)
                    telegramCommandsChangeSession.RegisterCommand(item.Command, item.Description, BotCommandScope.Default());
                await telegramCommandsChangeSession.ApplyChanges();
            }
        }
    }
}
