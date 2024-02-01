using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Users;
public interface IUserContext
{
    public ChatId ScopeChatID { get; }
    public ChatId RequestChatID { get; }

    public Task<Chat> ScopeChat { get; }

    public Task<Chat> RequestChat { get; }

    public User ScopeUser { get; }
}
