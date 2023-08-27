using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class TGChat : Update
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; }

        public ISession Session { get; internal set; }

        public IServiceProvider UserService => __UserServiceScope.ServiceProvider;
        private readonly IServiceScope __UserServiceScope;

        private TGChat(IServiceScope serviceScope, ChatId chatId)
        {
            __UserServiceScope = serviceScope;
            ChatId = chatId;

            Session = UserService.GetService<ISession>();
        }

        public static TGChat GetChat(ITelegramBotClient telegramBot, ChatId chatId, IServiceProvider BotService)
        {
            TGChat chat = new(BotService.CreateScope(), chatId)
            {
                BotClient = telegramBot,
            };
            return chat;
        }
    }
}
