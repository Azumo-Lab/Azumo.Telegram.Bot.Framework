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

using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Reflections
{
    /// <summary>
    /// 一个指令Model
    /// </summary>
    internal class BotCommand
    {
        /// <summary>
        /// Key 指令名称
        /// </summary>
        public string BotCommandName { get; set; }

        /// <summary>
        /// Key 消息类型
        /// </summary>
        public MessageType? MessageType { get; set; }

        /// <summary>
        /// 指令的执行委托
        /// </summary>
        public Func<TelegramController, object[], Task> Command { get; set; }

        /// <summary>
        /// 指令的参数
        /// </summary>
        public List<BotCommandParams> BotCommandParams { get; set; }

        /// <summary>
        /// 控制器的类型
        /// </summary>
        public Type ControllerType { get; set; }
    }
}
