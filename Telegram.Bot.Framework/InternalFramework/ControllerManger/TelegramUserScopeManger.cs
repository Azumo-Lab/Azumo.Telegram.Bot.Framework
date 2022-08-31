//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Framework.InternalFramework.ControllerManger/>
//
//  This program is free software: you can redistribute it and/or modify
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
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.InternalFramework.ControllerManger
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramUserScopeManger : ITelegramUserScopeManger
    {
        private readonly IServiceProvider serviceProvider;
        private readonly static Dictionary<long, ITelegramUserScope> User_Controller = new Dictionary<long, ITelegramUserScope>();
        private readonly static Dictionary<long, (int Count, DateTime LastUseTime)> User_Time = new Dictionary<long, (int Count, DateTime LastUseTime)>();

        public TelegramUserScopeManger(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITelegramUserScope GetTelegramUserScope()
        {
            TelegramContext context = serviceProvider.GetService<TelegramContext>();
            if (!User_Controller.ContainsKey(context.ChatID))
            {
                ITelegramUserScope telegramUserScope = serviceProvider.GetService<ITelegramUserScope>();
                User_Controller.Add(context.ChatID, telegramUserScope);
                ClearOldUser();
            }
            if (!User_Time.ContainsKey(context.ChatID))
                User_Time.Add(context.ChatID, (0, DateTime.Now));

            (int Count, DateTime _) = User_Time[context.ChatID];
            User_Time[context.ChatID] = (Count += 1, DateTime.Now);

            return User_Controller[context.ChatID];
        }

        private void ClearOldUser()
        {
            List<long> ClearUser = new List<long>();
            DateTime Before24H = DateTime.Now.AddDays(1);
            foreach (var item in User_Time)
            {
                if (item.Value.LastUseTime < Before24H)
                {
                    ClearUser.Add(item.Key);
                }
            }

            ClearUser.ForEach(x =>
            {
                User_Controller.Remove(x);
                User_Time.Remove(x);
            });
        }
    }
}
