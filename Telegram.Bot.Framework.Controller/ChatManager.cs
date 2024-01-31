using Azumo.Utils;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller;

/// <summary>
/// 
/// </summary>
[DependencyInjectionSingleton(ServiceType = typeof(IChatManager))]
internal class ChatManager : IChatManager
{
    /// <summary>
    /// 用于缓存 <see cref="TelegramUserChatContext"/> 对象
    /// </summary>
    private readonly Dictionary<long, TelegramUserChatContext> __UserIDs = [];

    /// <summary>
    /// 创建或取得 <see cref="TelegramUserChatContext"/> 对象
    /// </summary>
    /// <remarks>
    /// 当缓存中没有指定用户的数据时，将创建一个新的 <see cref="TelegramUserChatContext"/> 对象，
    /// 如果缓存中已经有了 <see cref="TelegramUserChatContext"/> 对象，则从缓存中取出，并更新值
    /// </remarks>
    /// <param name="telegramBotClient">机器人接口</param>
    /// <param name="update">更新数据</param>
    /// <param name="BotServiceProvider">Bot级别的服务</param>
    /// <returns><see cref="TelegramUserChatContext"/> 对象</returns>
    public TelegramUserChatContext Create(ITelegramBotClient telegramBotClient, Update update, IServiceProvider BotServiceProvider)
    {
        var User = update.GetRequestUser();
        if (User == null) return null!;

        var userID = User.Id;

        if (!__UserIDs.TryGetValue(userID, out var chatContext))
        {
            chatContext = TelegramUserChatContext.GetChat(User, BotServiceProvider);
            _ = __UserIDs.TryAdd(userID, chatContext);
        }
        update.CopyTo(chatContext);
        return chatContext;
    }
}
