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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalInterface
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IControllerManager))]
    internal class ControllerManager : IControllerManager
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BotCommand> InternalCommands { get; } = [];

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, BotCommand> __BotCommand = [];

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<MessageType, BotCommand> __BotCommandMessageType = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tGChat"></param>
        /// <returns></returns>
        public BotCommand GetCommand(TGChat tGChat)
        {
            string command;
            if ((command = tGChat.GetCommand()) != null)
            {
                if (__BotCommand.TryGetValue(command, out var botCommand))
                    return botCommand;
                botCommand = InternalCommands.Where(x => x.BotCommandName == command).FirstOrDefault();
                if (botCommand != null)
                    _ = __BotCommand.TryAdd(command, botCommand);
                return botCommand!;
            }
            else
            {
                var type = tGChat.Message?.Type ?? MessageType.Unknown;
                if (__BotCommandMessageType.TryGetValue(type, out var botCommand))
                    return botCommand;

                botCommand = InternalCommands.Where(x => x.MessageType == type).FirstOrDefault();
                if (botCommand != null)
                    _ = __BotCommandMessageType.TryAdd(type, botCommand);
                return botCommand!;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BotCommand> GetAllCommands() => new(InternalCommands);
    }
}
