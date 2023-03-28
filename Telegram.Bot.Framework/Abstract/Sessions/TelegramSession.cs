using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Users;
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
        private readonly IServiceScope __ServiceScope;

        /// <summary>
        /// 用户服务
        /// </summary>
        public IServiceProvider UserService => __ServiceScope.ServiceProvider;

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

            return user == null
                ? default
                : new TelegramUser
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
            return update.Type switch
            {
                Types.Enums.UpdateType.Message => update?.Message?.Chat.Id,
                Types.Enums.UpdateType.CallbackQuery => update?.CallbackQuery?.Message?.Chat?.Id,
                Types.Enums.UpdateType.EditedMessage => update?.EditedMessage?.Chat.Id,
                Types.Enums.UpdateType.ChannelPost => update?.ChannelPost?.Chat.Id,
                Types.Enums.UpdateType.EditedChannelPost => update?.EditedChannelPost?.Chat.Id,
                Types.Enums.UpdateType.MyChatMember => update?.MyChatMember?.Chat.Id,
                Types.Enums.UpdateType.ChatMember => update?.ChatMember?.Chat?.Id,
                Types.Enums.UpdateType.ChatJoinRequest => update?.ChatJoinRequest?.Chat?.Id,
                Types.Enums.UpdateType.Poll or Types.Enums.UpdateType.Unknown or Types.Enums.UpdateType.PollAnswer or Types.Enums.UpdateType.InlineQuery or Types.Enums.UpdateType.ShippingQuery or Types.Enums.UpdateType.PreCheckoutQuery or Types.Enums.UpdateType.ChosenInlineResult => default!,
                _ => default!,
            };
        }

        public static User? GetUser(Update update)
        {
            return update.Type switch
            {
                Types.Enums.UpdateType.Message => update?.Message?.From,
                Types.Enums.UpdateType.InlineQuery => update?.InlineQuery?.From,
                Types.Enums.UpdateType.ChosenInlineResult => update?.ChosenInlineResult?.From,
                Types.Enums.UpdateType.CallbackQuery => update?.CallbackQuery?.From,
                Types.Enums.UpdateType.EditedMessage => update?.EditedMessage?.From,
                Types.Enums.UpdateType.ChannelPost => update?.ChannelPost?.From,
                Types.Enums.UpdateType.EditedChannelPost => update?.EditedChannelPost?.From,
                Types.Enums.UpdateType.ShippingQuery => update?.ShippingQuery?.From,
                Types.Enums.UpdateType.PreCheckoutQuery => update?.PreCheckoutQuery?.From,
                Types.Enums.UpdateType.PollAnswer => update?.PollAnswer?.User,
                Types.Enums.UpdateType.MyChatMember => update?.MyChatMember?.From,
                Types.Enums.UpdateType.ChatMember => update?.ChatMember?.From,
                Types.Enums.UpdateType.ChatJoinRequest => update?.ChatJoinRequest?.From,
                Types.Enums.UpdateType.Poll or Types.Enums.UpdateType.Unknown => default!,
                _ => default!,
            };
        }

        #endregion

    }
}
