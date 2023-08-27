using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Reflections
{
    internal static class BotCommandRoute
    {
        private static readonly Dictionary<string, BotCommand> __BotCommand = new Dictionary<string, BotCommand>();
        private static readonly Dictionary<MessageType, BotCommand> __MessageType = new Dictionary<MessageType, BotCommand>();
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
                if(command.MessageType == null)
                    throw new ArgumentNullException(nameof(command.MessageType));

                __MessageType.Add((MessageType)command.MessageType, command);
            }
        }

        public static BotCommand GetBotCommand(string BotCommand)
        {
            if (string.IsNullOrEmpty(BotCommand))
                return null;

            __BotCommand.TryGetValue(BotCommand, out BotCommand botCommand);
            return botCommand;
        }
        public static BotCommand GetBotCommand(MessageType type)
        {
            __MessageType.TryGetValue(type, out BotCommand botCommand);
            return botCommand;
        }
    }
}
