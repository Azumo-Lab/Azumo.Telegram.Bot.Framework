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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalImplementation.Managements
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Transient, ServiceType = typeof(IPrivateChat))]
    internal class PrivateChat : IPrivateChat
    {
        private DateTime BanTime = DateTime.MinValue;
        private TimeSpan TimeSpan = TimeSpan.Zero;
        /// <summary>
        /// 
        /// </summary>
        public long ChatID { get; set; }

        private bool _Ban;
        /// <summary>
        /// 
        /// </summary>
        public bool IsBan
        {
            get
            {
                if (BanTime + TimeSpan < DateTime.Now)
                    return true;
                return _Ban;
            }
            set
            {
                _Ban = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient TelegramBotClient { get; set; }

        public ChatType ChatType { get; set; }

        public IServiceScope ChatServiceScope { get; set; }

        public ISession Session { get; set; }

        public ITelegramRequest TelegramRequest { get; set; }

        public Task Ban()
        {
            IsBan = true;
            return Task.CompletedTask;
        }

        public Task Ban(TimeSpan timeSpan)
        {
            BanTime = DateTime.Now;
            TimeSpan = timeSpan;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ChatServiceScope.Dispose();
        }
    }
}
