using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Core.Users;

/// <summary>
/// 
/// </summary>
public sealed class TelegramUserContext : Update, IDisposable, IUserContext
{
    private readonly AsyncServiceScope UserServiceScope;

    public IServiceProvider UserServiceProvider => UserServiceScope.ServiceProvider;

    public ITelegramBotClient BotClient { get; }

    public ISession Session { get; }

    public IPrivateStorage Storage { get; }

    public ChatId ScopeChatID => Storage.UserInfo.Id;

    public ChatId RequestChatID => TelegramUserContextExtensions.GetChatID(this);

    public Task<Chat> ScopeChat => BotClient.GetChatAsync(ScopeChatID);

    public Task<Chat> RequestChat => BotClient.GetChatAsync(RequestChatID);

    public User? ScopeUser { get; set; }

    public TelegramUserContext(IServiceProvider serviceProvider)
    {
        UserServiceScope = serviceProvider.CreateAsyncScope();

        BotClient = UserServiceProvider.GetRequiredService<ITelegramBotClient>();
        Session = UserServiceProvider.GetRequiredService<ISession>();
        Storage = UserServiceProvider.GetRequiredService<IPrivateStorage>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<string>? GetCommand()
    {
        var entitiy = Message?.Entities?.FirstOrDefault();
        return entitiy?.Type != MessageEntityType.BotCommand
            ? null
            : Message!.Entities!.Length == 1
            ? (List<string>)([Message!.EntityValues!.First()])
            : Message!.EntityValues!.ToList();
    }

    public async void Dispose()
    {
        UserServiceScope.Dispose();
        await UserServiceScope.DisposeAsync();
    }
}
