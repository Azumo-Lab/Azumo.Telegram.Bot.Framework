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

using Telegram.Bot.Framework.Abstracts.Bot;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 机器人指令标签
    /// </summary>
    /// <remarks>
    /// 机器人的指令标签，将标签放置在方法上方，设置好指令之后，在Bot中输入相应的指令，即可调用相应的方法。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : Attribute
    {
        /// <summary>
        /// 指令，以 '/start' 形式发送的指令
        /// </summary>
        /// <remarks>
        /// 指令的名称，可以输入例如：`/Start`, `Start`, `start`, `START`等任意大小写，带`/`的指令。
        /// </remarks>
        public string Command { get; } = default!;

        /// <summary>
        /// 指令的简单描述
        /// </summary>
        /// <remarks>
        /// 指令的描述，默认值为中文，`没有描述`
        /// </remarks>
        public string Description { get; set; } = LanguageStatic.Instance[LanguageKey.DefaultCommandDetails];

        /// <summary>
        /// 一个值，指示是否将这个命令注册到Telegram Bot中
        /// </summary>
        public bool Register { get; set; } = true;

        /// <summary>
        /// 该指令能够使用的范围
        /// </summary>
        public BotCommandScopeType CommandScope { get; set; } = BotCommandScopeType.Default;

        /// <summary>
        /// 机器人指令标签
        /// </summary>
        /// <remarks>
        /// <paramref name="Command"/> 指令的名称，可以输入例如：`/Start`, `Start`, `start`, `START`等任意大小写，带`/`的指令。
        /// </remarks>
        /// <param name="Command">指令，以 '/start' 形式发送的指令</param>
        public BotCommandAttribute(string Command)
        {
            if (!Command.StartsWith("/"))
            {
                Command = $"/{Command}";
            }
            this.Command = Command;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class BotCommandAttributeEX
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseBotCommandAttribute(this IServiceCollection services)
        {
            List<Type> controllers = typeof(TelegramController).FindTypes(true);
            return services;
        }
    }
}
