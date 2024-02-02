using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Filters;
public interface IContextFilter
{
    public bool Filter(TelegramUserContext userContext);
}
