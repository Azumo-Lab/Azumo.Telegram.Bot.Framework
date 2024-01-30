using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.Controller;

/// <summary>
/// 
/// </summary>
public abstract class BaseParameterGetter
{
    public EnumReadParam Result { get; private set; } = EnumReadParam.None;

    public void Init() => 
        Result = EnumReadParam.None;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task SendPromptMessage(TelegramUserChatContext context)
    {
        try
        {
            await Send(context);
            Result = EnumReadParam.WaitInput;
        }
        catch (Exception)
        {
            Result = EnumReadParam.OK;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public object? GetParam(TelegramUserChatContext context)
    {
        try
        {
            return GetParamObj(context);
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            Result = EnumReadParam.OK;
        }
    }

    protected abstract Task Send(TelegramUserChatContext context);
    protected abstract object? GetParamObj(TelegramUserChatContext context);
}
