using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Users;

/// <summary>
/// 
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// 
    /// </summary>
    public ChatId ScopeChatID { get; }

    /// <summary>
    /// 
    /// </summary>
    public ChatId RequestChatID { get; }

    /// <summary>
    /// 
    /// </summary>
    public Task<Chat> ScopeChat { get; }

    /// <summary>
    /// 
    /// </summary>
    public Task<Chat> RequestChat { get; }

    /// <summary>
    /// 
    /// </summary>
    public User? ScopeUser { get; }
}
