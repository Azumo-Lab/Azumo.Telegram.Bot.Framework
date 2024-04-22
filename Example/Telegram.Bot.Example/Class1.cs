using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Example;

[DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IChatMemberChange))]
internal class Class1 : IChatMemberChange
{

    public async Task ChatMemberChangeAsync(TelegramUserContext telegramUserContext, ChatMember newChatMember, ChatId fromUser, ChatId user)
    {
        var chat = await telegramUserContext.BotClient.GetChatAsync(fromUser);
        await telegramUserContext.BotClient.SendTextMessageAsync(user, $"感谢老铁的邀请： @{chat.Username}");
        await Task.CompletedTask;
    }
}
