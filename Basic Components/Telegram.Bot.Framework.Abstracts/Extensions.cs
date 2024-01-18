using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts;

public static class Extensions
{
    #region 这里是对 IServiceCollection 接口进行的扩展
    /// <summary>
    /// 对所有的服务进行替换
    /// </summary>
    /// <remarks>
    /// 替换所有 <typeparamref name="TService"/> 类型的服务，并添加新的服务
    /// </remarks>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类型</typeparam>
    /// <param name="serviceDescriptors">服务</param>
    /// <param name="serviceLifetime">服务周期</param>
    /// <returns><see cref="IServiceCollection"/>服务</returns>
    /// <exception cref="ArgumentException"><see cref="ServiceLifetime"/>服务周期枚举出现异常</exception>
    public static IServiceCollection ReplaceAll<TService, TImplementation>(this IServiceCollection serviceDescriptors, ServiceLifetime serviceLifetime)
        where TService : class
        where TImplementation : class, TService
    {
        _ = serviceDescriptors.RemoveAll<TService>();
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                serviceDescriptors.TryAddSingleton<TService, TImplementation>();
                break;
            case ServiceLifetime.Scoped:
                serviceDescriptors.TryAddScoped<TService, TImplementation>();
                break;
            case ServiceLifetime.Transient:
                serviceDescriptors.TryAddTransient<TService, TImplementation>();
                break;
            default:
                throw new ArgumentException($"Not Found {nameof(serviceLifetime)} Value, Value : {(int)serviceLifetime}");
        }
        return serviceDescriptors;
    }
    #endregion

    #region 对缓存接口 ISession 进行扩展
    private static readonly string BOT_COMMAND_NAME = Guid.NewGuid().ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public static Controllers.BotCommand? GetBotCommand(this ISession session) =>
        session.Get<Controllers.BotCommand>(BOT_COMMAND_NAME);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    /// <param name="command"></param>
    public static void SetBotCommand(this ISession session, Controllers.BotCommand command) =>
        _ = session.Set(BOT_COMMAND_NAME, command);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    public static void RemoveBotCommand(this ISession session) =>
        _ = session.Remove(BOT_COMMAND_NAME);

    /// <summary>
    /// 设置字符串类型数据
    /// </summary>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string SetString(this ISession session, string key, string value)
    {
        _ = session.Set(key, value);
        return value;
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetString(this ISession session, string key) =>
        (session.Get(key) as string) ?? string.Empty;

    /// <summary>
    /// 获取指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T? Get<T>(this ISession session, string key)
    {
        object result;
        return (result = session.Get(key)) != null && result is T tResult ? tResult : default;
    }

    public static T Set<T>(this ISession session, string key, T value)
    {
        _ = session.Set(key, value!);
        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryGetValue<T>(this ISession session, string key, out T? value)
    {
        bool result;
        value = default;
        if (result = session.TryGetValue(key, out var outResult))
            value = outResult is T tResult ? tResult : default;
        return result;
    }
    #endregion

    #region Update 的扩展方法
    /// <summary>
    /// 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static User? GetRequestUser(this Update update) => update.Type switch
    {
        Types.Enums.UpdateType.Unknown => null,
        Types.Enums.UpdateType.Message => update.Message?.From,
        Types.Enums.UpdateType.InlineQuery => null,
        Types.Enums.UpdateType.ChosenInlineResult => null,
        Types.Enums.UpdateType.CallbackQuery => null,
        Types.Enums.UpdateType.EditedMessage => update.EditedMessage?.From,
        Types.Enums.UpdateType.ChannelPost => update.ChannelPost?.From,
        Types.Enums.UpdateType.EditedChannelPost => update.EditedChannelPost?.From,
        Types.Enums.UpdateType.ShippingQuery => null,
        Types.Enums.UpdateType.PreCheckoutQuery => null,
        Types.Enums.UpdateType.Poll => null,
        Types.Enums.UpdateType.PollAnswer => null,
        Types.Enums.UpdateType.MyChatMember => update.MyChatMember?.From,
        Types.Enums.UpdateType.ChatMember => update.ChatMember?.From,
        Types.Enums.UpdateType.ChatJoinRequest => update.ChatJoinRequest?.From,
        _ => throw new NotImplementedException(),
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Chat? GetRequestChat(this Update update) => update.Type switch
    {
        Types.Enums.UpdateType.Unknown => null,
        Types.Enums.UpdateType.Message => update.Message?.Chat,
        Types.Enums.UpdateType.InlineQuery => null,
        Types.Enums.UpdateType.ChosenInlineResult => null,
        Types.Enums.UpdateType.CallbackQuery => null,
        Types.Enums.UpdateType.EditedMessage => update.EditedMessage?.Chat,
        Types.Enums.UpdateType.ChannelPost => update.ChannelPost?.Chat,
        Types.Enums.UpdateType.EditedChannelPost => update.EditedChannelPost?.Chat,
        Types.Enums.UpdateType.ShippingQuery => null,
        Types.Enums.UpdateType.PreCheckoutQuery => null,
        Types.Enums.UpdateType.Poll => null,
        Types.Enums.UpdateType.PollAnswer => null,
        Types.Enums.UpdateType.MyChatMember => update.MyChatMember?.Chat,
        Types.Enums.UpdateType.ChatMember => update.ChatMember?.Chat,
        Types.Enums.UpdateType.ChatJoinRequest => update.ChatJoinRequest?.Chat,
        _ => throw new NotImplementedException(),
    };
    #endregion
}
