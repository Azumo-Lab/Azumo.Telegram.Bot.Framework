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
    [DependencyInjection(ServiceLifetime.Transient, ServiceType = typeof(ITelegramCommandsChangeSession))]
    internal class TelegramCommandsChangeSession : ITelegramCommandsChangeSession
    {
        private static readonly Dictionary<BotCommandScope, List<BotCommand>> __BotCommands = new();
        private static readonly Dictionary<BotCommandScope, List<BotCommand>> __BotCommandsRemove = new();

        private readonly Dictionary<BotCommandScope, List<BotCommand>> __BotCommandsCopy = new();
        private readonly IServiceProvider serviceProvider;

        private bool DisposeFlag;

        public TelegramCommandsChangeSession(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommands)
                __BotCommandsCopy.Add(item.Key, item.Value);
        }

        public async Task ApplyChanges()
        {
            CheckDisposeFlag();

            __BotCommands.Clear();
            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommandsCopy)
                __BotCommands.Add(item.Key, item.Value);

            ITelegramBotClient botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();
            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommandsCopy)
                await botClient.SetMyCommandsAsync(item.Value, item.Key);
        }

        public void ChangeScope(string commandName, BotCommandScope botCommandScope)
        {
            CheckDisposeFlag();

            BotCommand botCommand = null;
            BotCommandScope commandScope = null;
            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommandsCopy)
            {
                botCommand = item.Value.Where(x => x.Command == commandName).FirstOrDefault();
                if (botCommand != null)
                {
                    commandScope = item.Key;
                    break;
                }
            }
            if(botCommand != null && commandScope != null)
            {
                __BotCommandsCopy[commandScope].Remove(botCommand);
                if (__BotCommandsCopy.ContainsKey(botCommandScope))
                    __BotCommandsCopy[botCommandScope].Add(botCommand);
                else
                    __BotCommandsCopy.Add(botCommandScope, new List<BotCommand> { botCommand });
            }
        }

        public void Dispose()
        {
            if (!DisposeFlag)
            {
                DisposeFlag = true;
                __BotCommandsCopy.Clear();
            }
        }

        public void RegisterCommand(string commandName, string commandInfo, BotCommandScope botCommandScope = null)
        {
            CheckDisposeFlag();

            if (botCommandScope.IsNull())
                botCommandScope = BotCommandScope.Default();
            if(__BotCommandsCopy.TryGetValue(botCommandScope, out List<BotCommand> botCommands))
            {
                botCommands.Add(new BotCommand
                {
                    Command = commandName,
                    Description = commandInfo,
                });
            }
            else
            {
                botCommands = new List<BotCommand>()
                {
                    new BotCommand
                    {
                        Command = commandName,
                        Description = commandInfo,
                    }
                };
                __BotCommandsCopy.Add(botCommandScope, botCommands);
            }
        }

        public void RemoveCommand(string commandName)
        {
            CheckDisposeFlag();

            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommandsCopy)
            {
                BotCommand botCommand;
                if ((botCommand = item.Value.Where(x => x.Command == commandName).FirstOrDefault()) != null)
                {
                    if (__BotCommandsRemove.ContainsKey(item.Key))
                        __BotCommandsRemove[item.Key].Add(botCommand);
                    else
                        __BotCommandsRemove.Add(item.Key, new List<BotCommand> { botCommand });
                    item.Value.Remove(botCommand);
                }
            }
        }

        public void RestoreCommand(string commandName)
        {
            CheckDisposeFlag();

            foreach (KeyValuePair<BotCommandScope, List<BotCommand>> item in __BotCommandsRemove)
            {
                BotCommand botCommand;
                if ((botCommand = item.Value.Where(x => x.Command == commandName).FirstOrDefault()) != null)
                {
                    if (__BotCommandsCopy.ContainsKey(item.Key))
                        __BotCommandsCopy[item.Key].Add(botCommand);
                    else
                        __BotCommandsCopy.Add(item.Key, new List<BotCommand> { botCommand });
                    item.Value.Remove(botCommand);
                }
            }
        }

        private void CheckDisposeFlag()
        {
            if (DisposeFlag)
                throw new ObjectDisposedException(nameof(ITelegramCommandsChangeSession));
        }
    }
}
