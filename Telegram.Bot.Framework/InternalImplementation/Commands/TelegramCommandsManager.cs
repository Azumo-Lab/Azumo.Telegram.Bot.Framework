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
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.InternalImplementation.Commands
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class TelegramCommandsManager : ITelegramCommandsManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITelegramBotClient botClient;

        private readonly Dictionary<BotCommandScope, List<BotCommand>> __BotCommandsDic = new();

        public TelegramCommandsManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            botClient = this.serviceProvider.GetRequiredService<ITelegramBotClient>();
        }

        public ITelegramCommandsChangeSession ChangeTelegramCommands()
        {
            return serviceProvider.GetService<ITelegramCommandsChangeSession>();
        }

        public bool ContainsCommand(string commandName)
        {
            return GetBotCommand(commandName) != null;
        }

        public BotCommand GetBotCommand(string commandName)
        {
            return GetBotCommands().Where(x => x.Command == commandName).FirstOrDefault();
        }

        public List<BotCommand> GetBotCommands()
        {
            return __BotCommandsDic.Values.SelectMany(x => x).ToList();
        }

        public List<BotCommand> GetBotCommands(BotCommandScope botCommandScope)
        {
            if (!__BotCommandsDic.TryGetValue(botCommandScope, out List<BotCommand> botCommands))
                botCommands = new List<BotCommand>();
             return botCommands;
        }

        public async Task RegisterCommand(List<BotCommand> command, BotCommandScope botCommandScope = null)
        {
            if (!botCommandScope.IsNull())
            {
                if (__BotCommandsDic.ContainsKey(botCommandScope))
                {
                    __BotCommandsDic[botCommandScope].Clear();
                    __BotCommandsDic[botCommandScope].AddRange(command);
                }
                else
                {
                    __BotCommandsDic.Add(botCommandScope, command);
                }
            }

            await botClient.SetMyCommandsAsync(command, botCommandScope);
        }
    }
}
