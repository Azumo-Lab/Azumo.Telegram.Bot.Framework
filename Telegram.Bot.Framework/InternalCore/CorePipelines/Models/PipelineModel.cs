using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework.InternalCore.CorePipelines.Models;
/// <summary>
/// 
/// </summary>
internal class PipelineModel
{
    /// <summary>
    /// 
    /// </summary>
    public required TelegramUserContext UserContext { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required ICommandManager CommandManager { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required ICommandScopeService CommandScopeService { get; set; }
}
