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

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 设定哪些Bot可以访问
    /// </summary>
    /// <remarks>
    /// 加上这个标签之后，需要Bot的名称符合设定的Bot名称之后，才可以正常进行访问
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class BotNameAttribute : Attribute
    {
        /// <summary>
        /// 设定的Bot名称，可以有多条数据
        /// </summary>
        public HashSet<string> BotNames { get; }

        /// <summary>
        /// 对名称进行设定
        /// </summary>
        /// <param name="BotNames">要设定的Bot名称</param>
        public BotNameAttribute(params string[] BotNames)
        {
            this.BotNames = new HashSet<string>(BotNames);
        }
    }
}
