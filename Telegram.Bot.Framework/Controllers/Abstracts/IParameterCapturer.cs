using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers.Abstracts;

/// <summary>
/// 
/// </summary>
public interface IParameterCapturer
{
    /// <summary>
    /// 
    /// </summary>
    public EnumParametersResults Results { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="telegramUserChatContext"></param>
    /// <returns></returns>
    public Task SendMessageAsync(TelegramUserChatContext telegramUserChatContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="telegramUserChatContext"></param>
    /// <returns></returns>
    public object? GetParam(TelegramUserChatContext telegramUserChatContext);

    /// <summary>
    /// 
    /// </summary>
    public void Init();
}
