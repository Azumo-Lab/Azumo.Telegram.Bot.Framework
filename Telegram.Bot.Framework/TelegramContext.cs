using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    public class TelegramContext
    {
        public ITelegramBotClient BotClient { get; }
        public Update Update { get; }
        public CancellationToken CancellationToken { get; }

        internal TelegramContext(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken)
        {
            this.BotClient = BotClient;
            this.Update = Update;
            this.CancellationToken = CancellationToken;
        }

        public long ChatID => Update.Message.Chat.Id;

        public string GetCommand()
        {
            string command;
            if (!string.IsNullOrEmpty(command = Update.Message.Text))
                if (!command.StartsWith('/'))
                    command = null;
            return command;
        }
    }
}
