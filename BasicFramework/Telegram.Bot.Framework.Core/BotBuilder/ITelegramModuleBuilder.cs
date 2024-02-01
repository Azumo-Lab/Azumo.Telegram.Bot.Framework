namespace Telegram.Bot.Framework.Core.BotBuilder;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ITelegramModuleBuilder<TResult>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public ITelegramModuleBuilder<TResult> AddModule(ITelegramModule module);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public TResult Build();
}
