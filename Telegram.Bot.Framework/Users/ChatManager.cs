using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Users
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IChatManager))]
    internal class ChatManager : IChatManager
    {
        private readonly Dictionary<ChatId, TGChat> __Chats = new();
        public TGChat Create(ITelegramBotClient telegramBotClient, Update update, IServiceProvider BotServiceProvider)
        {
            ChatId chatID = update.GetChatID();
            TGChat chat;
            if (chatID != null)
            {
                if (!__Chats.TryGetValue(chatID, out chat))
                {
                    chat = TGChat.GetChat(telegramBotClient, chatID, BotServiceProvider);
                    __Chats.Add(chatID, chat);
                }
            }
            else
                chat = TGChat.GetChat(telegramBotClient, chatID, BotServiceProvider);
            chat.CopyTo(update);
            return chat;
        }
    }
}
