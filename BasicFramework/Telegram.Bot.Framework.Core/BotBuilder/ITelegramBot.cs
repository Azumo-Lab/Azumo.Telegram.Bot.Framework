namespace Telegram.Bot.Framework.Core.BotBuilder;

/// <summary>
/// 
/// </summary>
public interface ITelegramBot : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task StartAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task StopAsync();
}
