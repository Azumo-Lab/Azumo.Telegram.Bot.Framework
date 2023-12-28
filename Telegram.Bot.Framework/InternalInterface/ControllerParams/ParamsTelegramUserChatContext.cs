using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalInterface.ControllerParams
{
    [TypeFor(typeof(TelegramUserChatContext))]
    internal class ParamsTelegramUserChatContext : BaseControllerParam
    {
        public override Task<object> CatchObjs(TelegramUserChatContext tGChat) =>
            Task.FromResult<object>(tGChat);

        public override Task<bool> SendMessage(TelegramUserChatContext tGChat, ParamAttribute paramAttribute) =>
            Task.FromResult(true);
    }
}
