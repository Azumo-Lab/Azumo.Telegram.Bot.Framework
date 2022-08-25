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

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="BotClient"></param>
        /// <param name="Update"></param>
        /// <param name="CancellationToken"></param>
        internal TelegramContext(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken)
        {
            this.BotClient = BotClient;
            this.Update = Update;
            this.CancellationToken = CancellationToken;
        }

        /// <summary>
        /// 获取ChatID
        /// </summary>
        public long ChatID => GetChatID();

        private long GetChatID()
        {
            return Update.Type switch
            {
                Types.Enums.UpdateType.CallbackQuery => Update.CallbackQuery.Message.Chat.Id,
                _ => Update.Message.Chat.Id,
            };
        }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <returns></returns>
        internal string GetCommand()
        {
            string command;
            if (!string.IsNullOrEmpty(command = Update.Message?.Text))
                if (!command.StartsWith('/'))
                    command = null;
            return command;
        }
    }
}
