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
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users;

/// <summary>
/// 
/// </summary>
public sealed partial class TelegramUserChatContext : Update, IDisposable
{
    /// <summary>
    /// 本次请求的ChatID
    /// </summary>
    public ChatId? RequestChatID => RequestChat?.Id;

    /// <summary>
    /// 用户的ChatID
    /// </summary>
    public ChatId UserChatID => User.UserChatID;

    /// <summary>
    /// 本次请求的Chat
    /// </summary>
    public Chat? RequestChat => Extensions.GetRequestChat(this);

    public User? RequestUser => Extensions.GetRequestUser(this);

    /// <summary>
    /// 用户的Chat
    /// </summary>
    public Chat UserChat => User.UserChat;

    /// <summary>
    /// 用户
    /// </summary>
    public User ThisUser => User.User;

    /// <summary>
    /// 机器人客户端接口
    /// </summary>
    public ITelegramBotClient BotClient { get; }

    /// <summary>
    /// 用户范围服务提供
    /// </summary>
    public IServiceProvider UserScopeService => __UserServiceScope.ServiceProvider;

    /// <summary>
    /// 用户服务
    /// </summary>
    public IUserServices UserServices { get; }

    /// <summary>
    /// 
    /// </summary>
    public ISession Session { get; }

    /// <summary>
    /// 
    /// </summary>
    private readonly AsyncServiceScope __UserServiceScope;

    /// <summary>
    /// 
    /// </summary>
    public IUser User { get; }

    /// <summary>
    /// 
    /// </summary>
    private bool __DisposedValue;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceScope"></param>
    /// <param name="chatId"></param>
    private TelegramUserChatContext(IServiceProvider service, User user)
    {
        __UserServiceScope = service.CreateAsyncScope();

        UserServices = UserScopeService.GetRequiredService<IUserServices>();
        BotClient = UserScopeService.GetRequiredService<ITelegramBotClient>();
        Session = UserScopeService.GetRequiredService<ISession>();
        User = UserScopeService.GetRequiredService<IUser>();

        User.User = user;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="telegramBot"></param>
    /// <param name="chatId"></param>
    /// <param name="BotService"></param>
    /// <returns></returns>
    public static TelegramUserChatContext GetChat(User user, IServiceProvider BotService) =>
        new(BotService, user);


    #region Dispose相关的方法
    private void Dispose(bool disposing)
    {
        if (!__DisposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                __UserServiceScope.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            __DisposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~TGChat()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
