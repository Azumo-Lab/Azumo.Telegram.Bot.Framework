using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
public interface IGetParam
{
    public ParamAttribute? ParamAttribute { get; }

    public Task SendMessage(TelegramUserContext context);

    public Task<object> GetParam(TelegramUserContext context);
}
