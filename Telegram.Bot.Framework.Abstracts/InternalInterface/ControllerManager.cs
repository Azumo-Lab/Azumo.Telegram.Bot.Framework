using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IControllerManager))]
    internal class ControllerManager : IControllerManager
    {
        public List<BotCommand> InternalCommands { get; } = new();

        public Dictionary<string, BotCommand> __BotCommand = new();
        public Dictionary<MessageType, BotCommand> __BotCommandMessageType = new();

        public BotCommand GetCommand(TGChat tGChat)
        {
            string command;
            if ((command = tGChat.GetCommand()) != null)
            {
                if (__BotCommand.TryGetValue(command, out BotCommand? botCommand))
                {
                    return botCommand;
                }

                botCommand = InternalCommands.Where(x => x.BotCommandName == command).FirstOrDefault();
                if (botCommand != null)
                {
                    _ = __BotCommand.TryAdd(command, botCommand);
                }

                return botCommand!;
            }
            else
            {
                MessageType type = tGChat.Message?.Type ?? MessageType.Unknown;
                if (__BotCommandMessageType.TryGetValue(type, out BotCommand? botCommand))
                {
                    return botCommand;
                }

                botCommand = InternalCommands.Where(x => x.MessageType == type).FirstOrDefault();
                if (botCommand != null)
                {
                    _ = __BotCommandMessageType.TryAdd(type, botCommand);
                }
                return botCommand!;
            }
        }

        public List<BotCommand> GetAllCommands()
        {
            return new List<BotCommand>(InternalCommands);
        }
    }
}
