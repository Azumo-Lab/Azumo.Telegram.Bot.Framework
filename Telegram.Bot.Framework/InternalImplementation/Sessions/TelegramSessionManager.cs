//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using System;
using System.Collections.Generic;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// TelegramSession管理
    /// </summary>
    internal sealed class TelegramSessionManager
    {
        /// <summary>
        /// 唯一的实例
        /// </summary>
        public static TelegramSessionManager Instance { get; } = new TelegramSessionManager();

        private readonly Dictionary<long, TelegramSession> UID_Session = new();
        private readonly Dictionary<long, TelegramSession> ChatID_Session = new();

        /// <summary>
        /// 获取TelegramSession
        /// </summary>
        /// <param name="serviceProvider">服务提供</param>
        /// <param name="update">Telegram请求</param>
        /// <returns>返回Session，可以为NULL</returns>
        public TelegramSession GetTelegramSession(IServiceProvider serviceProvider, Update update)
        {
            try
            {
                // 获取用户UID，使用UID来进行Session的存取
                long? id = TelegramSession.GetUser(update).Id;
                TelegramSession telegramSession = null!;
                // 请求中存在UID
                if (id != null)
                    // 获取Session
                    if (!UID_Session.TryGetValue(id.Value, out telegramSession))
                    {
                        // 没取到，是新用户
                        telegramSession = TelegramSession.CreateSession(serviceProvider, update);
                        // 保存数据
                        _ = UID_Session.TryAdd(id.Value, telegramSession);
                        _ = ChatID_Session.TryAdd(telegramSession.User.ChatID.Value, telegramSession);
                    }
                    // 请求中不存在UID，但是存在ChatID
                    else if ((id = TelegramSession.GetChatID(update)) != null)
                        // ChatID也无法获得
                        if (!ChatID_Session.TryGetValue(id.Value, out telegramSession))
                            return default!;
                // 如果已经取得成功，更新Update（请求数据）
                if (!telegramSession.IsNull())
                    telegramSession.Update = update;
                return telegramSession!;
            }
            catch (Exception)
            {
                // 途中抛出异常的话，返回空值
                return default!;
            }
        }
    }
}
