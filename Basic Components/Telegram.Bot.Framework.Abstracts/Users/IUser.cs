using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users;

public interface IUser
{
    public User User { get; set; }

    public Chat UserChat { get; }

    public ChatId UserChatID { get; }

    public ISession Session { get; }
}
