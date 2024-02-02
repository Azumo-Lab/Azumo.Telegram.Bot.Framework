using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
public abstract class BaseGetParam : IGetParam
{
    public ParamAttribute? ParamAttribute { get; }
    public abstract Task<object> GetParam(TelegramUserContext context);
    public virtual async Task SendMessage(TelegramUserContext context)
    {
        if (ParamAttribute?.IGetParmType != null)
        {
            if (ActivatorUtilities.CreateInstance(context.UserServiceProvider, ParamAttribute.IGetParmType, []) is IGetParam iGetParam)
            {
                await iGetParam.SendMessage(context);
                return;
            }
        }
        _ = await context.BotClient.SendTextMessageAsync(context.ScopeChatID, ParamAttribute?.Message ?? "请输入参数");
    }
}
