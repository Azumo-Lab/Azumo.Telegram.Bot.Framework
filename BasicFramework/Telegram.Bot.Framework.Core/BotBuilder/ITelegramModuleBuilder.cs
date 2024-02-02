namespace Telegram.Bot.Framework.Core.BotBuilder;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ITelegramModuleBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public ITelegramModuleBuilder AddModule(ITelegramModule module);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ITelegramBot Build();
}
