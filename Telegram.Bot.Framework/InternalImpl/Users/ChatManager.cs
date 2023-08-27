using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;
using TGChat = Telegram.Bot.Framework.Abstracts.Users.TGChat;

namespace Telegram.Bot.Framework.InternalImpl.Users
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IChatManager))]
    internal class ChatManager : IChatManager
    {
        private readonly Dictionary<ChatId, TGChat> __Chats = new Dictionary<ChatId, TGChat>();
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
            {
                chat = TGChat.GetChat(telegramBotClient, chatID, BotServiceProvider);
            }
            chat.SetUpdate(update);
            return chat;
        }
    }
}
