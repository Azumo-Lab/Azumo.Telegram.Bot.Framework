using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller;
public abstract class BaseGetParamDirect : IGetParam
{
    public ParamAttribute? ParamAttribute { get; set; }

    public abstract Task<object> GetParam(TelegramUserContext context);
    public Task<bool> SendMessage(TelegramUserContext context) =>
        Task.FromResult(true);
}
