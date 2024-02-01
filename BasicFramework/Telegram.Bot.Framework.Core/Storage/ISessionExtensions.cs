namespace Telegram.Bot.Framework.Core.Storage;
public static class ISessionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(this ISession session, object key) =>
        (T)session.Get(key);
}
