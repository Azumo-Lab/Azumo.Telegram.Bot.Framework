//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
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

namespace Telegram.Bot.Framework.InternalFramework
{
    internal class TelegramRouteController
    {
        private readonly TelegramContext TelegramContext;
        private readonly IServiceProvider ServiceProvider;
        private static readonly Dictionary<long, ITelegramRouteUserController> ChatIDUser = new Dictionary<long, ITelegramRouteUserController>();

        public TelegramRouteController(TelegramContext context, IServiceProvider serviceProvider)
        {
            TelegramContext = context;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <returns></returns>
        public async Task StartProcess()
        {
            long chatID = TelegramContext.ChatID;
            if (!ChatIDUser.ContainsKey(chatID))
            {
                ITelegramRouteUserController controller = ServiceProvider.GetService<ITelegramRouteUserController>();
                ChatIDUser.Add(chatID, controller);
            }
            await ChatIDUser[chatID].Invoke(TelegramContext, ServiceProvider);
        }
    }
}
