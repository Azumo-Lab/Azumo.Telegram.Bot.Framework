using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Users;

[DependencyInjection(ServiceLifetime.Scoped, typeof(IUser))]
internal class UserInfo(ITelegramBotClient telegramBotClient, ISession session) : IUser
{
    private User __User;
    private Chat __Chat;
    private ChatId __ChatID;

    public User User
    {
        get => __User;
        set
        {
            if (__User?.Id != value.Id)
            {
                __User = value;
                __Chat = null;
                __ChatID = null;
            }
        }
    }

    public Chat UserChat
    {
        get
        {
            __Chat ??= telegramBotClient.GetChatAsync(UserChatID).Result;
            return __Chat;
        }
    }

    public ChatId UserChatID
    {
        get
        {
            __ChatID ??= new ChatId(User.Id);
            return __ChatID;
        }
    }

    public ISession Session { get; set; } = session;
}
