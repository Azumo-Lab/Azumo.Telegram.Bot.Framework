using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Models;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    /// <summary>
    /// 访问的请求会话
    /// </summary>
    public sealed class TelegramSession : IDisposable
    {
        /// <summary>
        /// 是否已经关闭
        /// </summary>
        public bool IsDispose { get; private set; }

        /// <summary>
        /// 当前的用户
        /// </summary>
        public TelegramUser User { get; } = default!;

        /// <summary>
        /// Session操作类
        /// </summary>
        public ISession Session { get; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public Update Update { get; set; } = default!;

        /// <summary>
        /// 服务范围
        /// </summary>
        private IServiceScope __ServiceScope;

        /// <summary>
        /// 用户服务
        /// </summary>
        public IServiceProvider UserService
        {
            get
            {
                return __ServiceScope.ServiceProvider;
            }
        }

        /// <summary>
        /// 机器人Client
        /// </summary>
        public ITelegramBotClient BotClient { get; } = default!;

        /// <summary>
        /// 机器人控制
        /// </summary>
        public ITelegramBot TelegramBot { get; } = default!;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">用于创建该Session的服务</param>
        /// <param name="update">请求的信息</param>
        private TelegramSession(IServiceProvider ServiceProvider, Update update)
        {
            __ServiceScope = ServiceProvider.CreateScope();

            TelegramBot = UserService.GetRequiredService<ITelegramBot>();
            BotClient = UserService.GetRequiredService<ITelegramBotClient>();
            Session = UserService.GetRequiredService<ISession>();
            User = GetTelegramUser(update);
            Update = update;
        }

        /// <summary>
        /// 创建一个Session
        /// </summary>
        /// <param name="serviceProvider">服务</param>
        /// <param name="update">请求的信息</param>
        /// <returns>返回创建的Session</returns>
        public static TelegramSession CreateSession(IServiceProvider serviceProvider, Update update)
        {
            return new TelegramSession(serviceProvider, update);
        }

        /// <summary>
        /// 注销这个Session
        /// </summary>
        public void Dispose()
        {
            IsDispose = true;
            Session?.Dispose();
            __ServiceScope.Dispose();
        }

        #region 静态方法，获取用户信息

        public static TelegramUser GetTelegramUser(Update update)
        {
            User? user = GetUser(update);

            if (user == null)
                return default!;

            return new TelegramUser
            {
                ChatID = GetChatID(update),
                Id = user.Id,
                CanJoinGroups = user.CanJoinGroups,
                CanReadAllGroupMessages = user.CanReadAllGroupMessages,
                FirstName = user.FirstName,
                IsBot = user.IsBot,
                LanguageCode = user.LanguageCode,
                LastName = user.LastName,
                SupportsInlineQueries = user.SupportsInlineQueries,
                Username = user.Username,
            };
        }

        public static long? GetChatID(Update update)
        {
            switch (update.Type)
            {
                case Types.Enums.UpdateType.Message:
                    return update?.Message?.Chat.Id;
                case Types.Enums.UpdateType.CallbackQuery:
                    return update?.CallbackQuery?.Message?.Chat?.Id;
                case Types.Enums.UpdateType.EditedMessage:
                    return update?.EditedMessage?.Chat.Id;
                case Types.Enums.UpdateType.ChannelPost:
                    return update?.ChannelPost?.Chat.Id;
                case Types.Enums.UpdateType.EditedChannelPost:
                    return update?.EditedChannelPost?.Chat.Id;
                case Types.Enums.UpdateType.MyChatMember:
                    return update?.MyChatMember?.Chat.Id;
                case Types.Enums.UpdateType.ChatMember:
                    return update?.ChatMember?.Chat?.Id;
                case Types.Enums.UpdateType.ChatJoinRequest:
                    return update?.ChatJoinRequest?.Chat?.Id;
                case Types.Enums.UpdateType.Poll:
                case Types.Enums.UpdateType.Unknown:
                case Types.Enums.UpdateType.PollAnswer:
                case Types.Enums.UpdateType.InlineQuery:
                case Types.Enums.UpdateType.ShippingQuery:
                case Types.Enums.UpdateType.PreCheckoutQuery:
                case Types.Enums.UpdateType.ChosenInlineResult:
                    return default!;
                default:
                    return default!;
            }
        }

        public static User? GetUser(Update update)
        {
            switch (update.Type)
            {
                case Types.Enums.UpdateType.Message:
                    return update?.Message?.From;
                case Types.Enums.UpdateType.InlineQuery:
                    return update?.InlineQuery?.From;
                case Types.Enums.UpdateType.ChosenInlineResult:
                    return update?.ChosenInlineResult?.From;
                case Types.Enums.UpdateType.CallbackQuery:
                    return update?.CallbackQuery?.From;
                case Types.Enums.UpdateType.EditedMessage:
                    return update?.EditedMessage?.From;
                case Types.Enums.UpdateType.ChannelPost:
                    return update?.ChannelPost?.From;
                case Types.Enums.UpdateType.EditedChannelPost:
                    return update?.EditedChannelPost?.From;
                case Types.Enums.UpdateType.ShippingQuery:
                    return update?.ShippingQuery?.From;
                case Types.Enums.UpdateType.PreCheckoutQuery:
                    return update?.PreCheckoutQuery?.From;
                case Types.Enums.UpdateType.PollAnswer:
                    return update?.PollAnswer?.User;
                case Types.Enums.UpdateType.MyChatMember:
                    return update?.MyChatMember?.From;
                case Types.Enums.UpdateType.ChatMember:
                    return update?.ChatMember?.From;
                case Types.Enums.UpdateType.ChatJoinRequest:
                    return update?.ChatJoinRequest?.From;
                case Types.Enums.UpdateType.Poll:
                case Types.Enums.UpdateType.Unknown:
                    return default!;
                default:
                    return default!;
            }
        }

        #endregion

    }
}
