using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Interface
{
    public interface IFrameworkInfo
    {
        public List<CommandInfo> GetCommandInfos();

        public CommandInfo GetCommandInfo(string command);

        public CommandInfo GetCommandInfo(MessageType messageType);

        public CommandInfo GetCommandInfo(UpdateType messageType);
    }
}
