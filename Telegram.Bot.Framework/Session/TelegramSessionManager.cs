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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Types;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Session
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramSessionManager
    {
        public static TelegramSessionManager Instance { get; } = new TelegramSessionManager();

        private readonly Dictionary<long, TelegramSession> UID_Session = new Dictionary<long, TelegramSession>();
        private readonly Dictionary<long, TelegramSession> ChatID_Session = new Dictionary<long, TelegramSession>();
        public TelegramSession GetTelegramSession(IServiceProvider serviceProvider, Update update)
        {
            try
            {
                long? id = TelegramSession.GetUser(update).Id;
                TelegramSession telegramSession = null!;
                if (id != null)
                {
                    if (!UID_Session.TryGetValue(id.Value, out telegramSession))
                    {
                        telegramSession = TelegramSession.CreateSession(serviceProvider, update);
                        UID_Session.TryAdd(id.Value, telegramSession);
                        ChatID_Session.TryAdd(telegramSession.User.ChatID.Value, telegramSession);
                    }
                }
                else if ((id = TelegramSession.GetChatID(update)) != null)
                {
                    if (!ChatID_Session.TryGetValue(id.Value, out telegramSession))
                    {
                        return default!;
                    }
                }
                if (!telegramSession.IsNull())
                {
                    telegramSession.Update = update;
                }
                return telegramSession!;
            }
            catch (Exception)
            {
                return default!;
            }
        }
    }
}
