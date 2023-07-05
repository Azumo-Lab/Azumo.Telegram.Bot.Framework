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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.ExtensionMethods;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Transient)]
    internal class TelegramRequest : ITelegramRequest
    {
        public Update Update { get; set; }

        public IServiceScope BotScopeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCommand()
        {
            if (Update.IsNull())
                return string.Empty;

            string text = Update.Message?.Text;
            if (!text.IsNull() && text.StartsWith('/'))
                return text;

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Message GetMessage()
        {
            return Update.Message;
        }
    }
}
