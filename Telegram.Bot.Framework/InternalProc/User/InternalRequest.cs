using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    internal class InternalRequest : IRequest
    {
        public Update Update { get; set; }

        public InternalRequest(Update update)
        {
            Update = update;
        }

        public string GetCommand()
        {
            if (HasCommand())
                return Update.Message.Text;

            return string.Empty;
        }

        public Message GetMessage()
        {
            return Update.Message;
        }

        public bool HasCommand()
        {
            if (Update.Type != Types.Enums.UpdateType.Message)
                return false;

            string command = Update.Message?.Text ?? string.Empty;
            return command.StartsWith('/') && command.Length > 1;
        }
    }
}
