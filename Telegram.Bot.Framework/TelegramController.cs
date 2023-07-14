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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 控制器，是整个系统里面的关键的一个类
    /// </summary>
    /// <remarks>
    /// 想要实现一个指令，需要继承 这个类 <see cref="TelegramController"/>，之后，自定义一个方法：
    /// <code>
    /// public async Task Test()
    /// {
    ///     await Session.SendMessage("Hello")
    /// }
    /// </code>
    /// 这样定义之后，通过在方法上面设置 <see cref="BotCommandAttribute(string)"/> 之后即可使用指令，例如定义这样的指令：
    /// <code>
    /// [BotCommand("Test")]
    /// </code>
    /// 通过在Telegram中，向机器人发送指令 <c>/Test</c> 即可实现 回复 <c>Hello</c> 的功能<br/>
    /// 另外，需要注意的是：一旦方法上面设置 <see cref="BotCommandAttribute(string)"/> 之后，方法的参数也必须要设置 <see cref="ParamAttribute"/> 之后才能够正确执行参数的捕获操作
    /// </remarks>
    public abstract class TelegramController
    {
        protected ITelegramChat Chat { get; private set; }

        internal async Task Invoke(ITelegramChat _chat, Func<TelegramController, object[], Task> Action)
        {
            Chat = _chat;
            Message message = Chat.Request.GetMessage();
            string Command = Chat.CommandService.GetCommand();
            if (!await MessageFilter(message) ||
                !await CommandFilter(Command))
                return;

            await Action(this, Array.Empty<object>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual Task<bool> MessageFilter(Message message)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual Task<bool> CommandFilter(string command)
        {
            return Task.FromResult(true);
        }
    }
}
