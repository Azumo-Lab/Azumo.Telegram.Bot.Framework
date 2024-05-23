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
//
//  Author: 牛奶

using System;
using Telegram.Bot.Framework.Helpers;

namespace Telegram.Bot.Framework.Attributes
{
    /// <summary>
    /// 设置指令名称的标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class BotCommandAttribute : Attribute
    {
        /// <summary>
        /// 指令名称
        /// </summary>
        public string BotCommand { get; }

        /// <summary>
        /// 指令描述
        /// </summary>
        public string Description { get; set; } = "No details";

        /// <summary>
        /// 设置指令
        /// </summary>
        /// <param name="botCommand">指令名称</param>
        public BotCommandAttribute(string botCommand)
        {
            ExceptionHelper.ThrowIfNullOrEmpty(botCommand, nameof(botCommand));

            if (!botCommand.StartsWith('/'))
                botCommand = $"/{botCommand}";

            var botcommandItem = botCommand.ToLower();

            // 对指令进行验证
            for (var i = 0; i < botcommandItem.Length; i++)
            {
                var item = botcommandItem[i];
                if (!IsAscii(item))
                    throw new Exception("无法使用ASCII字符以外的字符进行设置指令");
                if (i == 0 && item != '/')
                    throw new Exception("没有使用'/'作为指令的开始");
                if (i == 1 && !IsAToZ(item))
                    throw new Exception("指令名称不可以使用数字或特殊符号开头");
                if (i != 0 && i != 1 && !IsAToZ(item) && !char.IsNumber(item) && item != '_')
                    throw new Exception("指令名称仅可以使用数字，字母，下划线");
            }
            if (botcommandItem.Length > 32)
                throw new Exception("指令名称过长，请不要超过32个字符");
            if (!string.IsNullOrEmpty(Description) && Description.Length > 256)
                throw new Exception("指令描述过长，请不要超过256个字符");

            BotCommand = botCommand.ToLower();
        }

        /// <summary>
        /// 验证是否是ASCII字符
        /// </summary>
        /// <param name="item">字符</param>
        /// <returns>是否</returns>
        private static bool IsAscii(char item) =>
#if NET8_0_OR_GREATER
            char.IsAscii(item);
#else
            item <= sbyte.MaxValue || item >= 0;
#endif

        /// <summary>
        /// 验证是否是A-Z或a-z
        /// </summary>
        /// <param name="item">字符</param>
        /// <returns>是否</returns>
        private static bool IsAToZ(char item) =>
#if NET8_0_OR_GREATER
            item is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
#else
            (item >= 'A' && item <= 'Z') || (item >= 'a' && item <= 'z');
#endif
    }
}
