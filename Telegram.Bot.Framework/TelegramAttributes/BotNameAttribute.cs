//  <Telegram.Bot.Framework>
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 设定Telegram Bot的名字
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BotNameAttribute : Attribute
    {
        /// <summary>
        /// Bot 的名称
        /// </summary>
        public string[] BotName { get; }

        public bool OverWrite { get; set; }

        public BotNameAttribute(params string[] BotName)
        {
            void Error()
            {
                throw new ArgumentNullException($"{nameof(BotName)} : is Null or Empty");
            }
            if (BotName == null || BotName.Length == 0)
                Error();
            foreach (var item in BotName)
                if (string.IsNullOrEmpty(item))
                    Error();

            this.BotName = BotName;
        }
    }
}
