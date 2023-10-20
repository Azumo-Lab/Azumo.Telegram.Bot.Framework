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

using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Reflections
{
    internal static class BotCommandRoute
    {
        private static readonly Dictionary<string, BotCommand> __BotCommand = new();
        private static readonly Dictionary<MessageType, BotCommand> __MessageType = new();
        public static void AddBotCommand(BotCommand command)
        {
            if (command.MessageType == null)
            {
                if (string.IsNullOrEmpty(command.BotCommandName))
                    throw new ArgumentNullException(nameof(command.BotCommandName));

                __BotCommand.Add(command.BotCommandName, command);
            }
            else if (string.IsNullOrEmpty(command.BotCommandName))
            {
                if (command.MessageType == null)
                    throw new ArgumentNullException(nameof(command.MessageType));

                __MessageType.Add((MessageType)command.MessageType, command);
            }
        }

        public static BotCommand GetBotCommand(string BotCommand)
        {
            if (string.IsNullOrEmpty(BotCommand))
                return null;

            _ = __BotCommand.TryGetValue(BotCommand.ToLower(), out BotCommand botCommand);
            return botCommand;
        }
        public static BotCommand GetBotCommand(MessageType type)
        {
            _ = __MessageType.TryGetValue(type, out BotCommand botCommand);
            return botCommand;
        }
    }
}
