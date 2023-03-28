using System.Collections.Generic;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Controller
{
    public interface ICommandManager
    {
        public List<CommandInfo> GetCommandInfos();

        public CommandInfo GetCommandInfo(string command);

        public CommandInfo GetCommandInfo(MessageType messageType);

        public CommandInfo GetCommandInfo(UpdateType messageType);
    }
}
