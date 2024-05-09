//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

namespace Telegram.Bot.Framework.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class BotCommandAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string BotCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; } = "No details";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botCommand"></param>
        public BotCommandAttribute(string botCommand)
        {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrEmpty(botCommand, nameof(botCommand));
#else
            if (string.IsNullOrEmpty(botCommand))
                throw new ArgumentNullException(nameof(botCommand));
#endif
            if (!botCommand.StartsWith('/'))
                botCommand = $"/{botCommand}";
            BotCommand = botCommand.ToLower();
        }
    }
}
