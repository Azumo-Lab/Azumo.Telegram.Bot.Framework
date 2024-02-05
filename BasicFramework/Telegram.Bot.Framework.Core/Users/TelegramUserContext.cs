//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

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

    public ChatId ScopeChatID => ScopeUser!.Id;

    public ChatId RequestChatID => TelegramUserContextExtensions.GetChatID(this);

    public Task<Chat> ScopeChat => BotClient.GetChatAsync(ScopeChatID);

    public Task<Chat> RequestChat => BotClient.GetChatAsync(RequestChatID);

    public User? ScopeUser { get; set; }

    public TelegramUserContext(IServiceProvider serviceProvider)
    {
        UserServiceScope = serviceProvider.CreateAsyncScope();

        BotClient = UserServiceProvider.GetRequiredService<ITelegramBotClient>();
        Session = UserServiceProvider.GetRequiredService<ISession>();
        Update += TelegramUserContext_Update;
    }

    private void TelegramUserContext_Update(object? sender, Update e)
    {

    }

    private event EventHandler<Update> Update;

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

    public void Copy(Update update)
    {
        Message = update.Message;
        InlineQuery = update.InlineQuery;
        MyChatMember = update.MyChatMember;

        Update?.Invoke(null, update);
    }
}
