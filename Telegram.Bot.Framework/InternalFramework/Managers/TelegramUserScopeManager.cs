﻿//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Abstract;

namespace Telegram.Bot.Framework.InternalFramework.Managers
{
    internal class TelegramUserScopeManager : ITelegramUserScopeManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<long, ITelegramUserScope> User_Controller = new();
        private readonly Dictionary<long, (int Count, DateTime LastUseTime)> User_Time = new();

        public TelegramUserScopeManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITelegramUserScope GetTelegramUserScope(long ChatID)
        {
            if (!User_Controller.ContainsKey(ChatID))
            {
                ITelegramUserScope telegramUserScope = serviceProvider.GetService<ITelegramUserScope>();
                User_Controller.Add(ChatID, telegramUserScope);
                ClearOldUser();
            }
            if (!User_Time.ContainsKey(ChatID))
                User_Time.Add(ChatID, (0, DateTime.Now));

            (int Count, DateTime _) = User_Time[ChatID];
            User_Time[ChatID] = (Count += 1, DateTime.Now);

            return User_Controller[ChatID];
        }

        /// <summary>
        /// 删除旧的过期数据
        /// </summary>
        private void ClearOldUser()
        {
            List<long> ClearUser = new List<long>();
            DateTime Before24H = DateTime.Now.AddDays(1);
            foreach (KeyValuePair<long, (int Count, DateTime LastUseTime)> item in User_Time)
                if (item.Value.LastUseTime < Before24H)
                    ClearUser.Add(item.Key);

            ClearUser.ForEach(x =>
            {
                User_Controller.Remove(x);
                User_Time.Remove(x);
            });
        }

        public IServiceScope GetUserScope(long ChatID)
        {
            if (User_Controller.ContainsKey(ChatID))
            {
                return User_Controller[ChatID].GetUserScope();
            }
            return null;
        }

        public ITelegramUserScope GetTelegramUserScope(TelegramUser telegramUser)
        {
            return GetTelegramUserScope(telegramUser.Id);
        }

        public IServiceScope GetUserScope(TelegramUser telegramUser)
        {
            return GetUserScope(telegramUser.Id);
        }
    }
}
