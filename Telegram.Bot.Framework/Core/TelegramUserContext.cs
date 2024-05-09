//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core
{
    /// <summary>
    /// TG用户上下文
    /// </summary>
    [DependencyInjection(ServiceLifetime.Transient, ServiceType = typeof(TelegramUserContext))]
    public sealed class TelegramUserContext : Update, IDisposable, IUserContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AsyncServiceScope UserServiceScope;

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider UserServiceProvider => UserServiceScope.ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; }

        /// <summary>
        /// 
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// 
        /// </summary>
        public ChatId ScopeChatID => ScopeUser!.Id;

        /// <summary>
        /// 
        /// </summary>
        public ChatId RequestChatID => this.GetChatID();

        /// <summary>
        /// 
        /// </summary>
        public Task<Chat> ScopeChat => BotClient.GetChatAsync(ScopeChatID);

        /// <summary>
        /// 
        /// </summary>
        public Task<Chat> RequestChat => BotClient.GetChatAsync(RequestChatID);

        /// <summary>
        /// 
        /// </summary>
        public User? ScopeUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TelegramUserContext(IServiceProvider serviceProvider)
        {
            UserServiceScope = serviceProvider.CreateAsyncScope();

            BotClient = UserServiceProvider.GetRequiredService<ITelegramBotClient>();
            Session = UserServiceProvider.GetRequiredService<ISession>();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<Update>? Update;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() =>
            UserServiceScope.Dispose();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        public void Copy(Update update)
        {
            Message = update.Message;
            InlineQuery = update.InlineQuery;
            MyChatMember = update.MyChatMember;
            CallbackQuery = update.CallbackQuery;
            ChannelPost = update.ChannelPost;
            ChatJoinRequest = update.ChatJoinRequest;
            ChatMember = update.ChatMember;
            ChosenInlineResult = update.ChosenInlineResult;

            Update?.Invoke(null, update);
        }
    }
}
