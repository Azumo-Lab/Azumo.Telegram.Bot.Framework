using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class CommandManager : ICommandManager
{

    private readonly Dictionary<string, IExecutor> CommandExecutor = [];

    private readonly Dictionary<MessageType, IExecutor> MessageTypeExecutor = [];

    public void AddExecutor(IExecutor executor)
    {
        BotCommandAttribute? botCommandAttribute;
        if ((botCommandAttribute = executor.Attributes.Where(x => x is BotCommandAttribute).Select(x => x as BotCommandAttribute).FirstOrDefault()) != null)
            CommandExecutor.Add(botCommandAttribute.BotCommand, executor);
    }
    public IExecutor? GetExecutor(TelegramUserContext userContext)
    {
        var commands = userContext.GetCommand();
        if (commands != null)
        {
            if (CommandExecutor.TryGetValue(commands[0], out var executor))
                return executor;
        }
        else
        {
            if (userContext?.Message != null && MessageTypeExecutor.TryGetValue(userContext.Message.Type, out var executor))
                return executor;
        }
        return null;
    }
}
