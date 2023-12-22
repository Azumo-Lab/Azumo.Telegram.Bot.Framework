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

namespace Telegram.Bot.Framework.Abstracts.Users
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TGChat : Update, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider UserService => __UserServiceScope.ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        public IAuthenticate Authenticate { get; }

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceScope __UserServiceScope;
        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScope"></param>
        /// <param name="chatId"></param>
        private TGChat(IServiceScope serviceScope, ChatId chatId)
        {
            __UserServiceScope = serviceScope;
            ChatId = chatId;

            Authenticate = UserService.GetRequiredService<IAuthenticate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramBot"></param>
        /// <param name="chatId"></param>
        /// <param name="BotService"></param>
        /// <returns></returns>
        public static TGChat GetChat(ITelegramBotClient telegramBot, ChatId chatId, IServiceProvider BotService)
        {
            TGChat chat = new(BotService.CreateScope(), chatId)
            {
                BotClient = telegramBot,
            };
            return chat;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    __UserServiceScope?.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
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
    }
}
