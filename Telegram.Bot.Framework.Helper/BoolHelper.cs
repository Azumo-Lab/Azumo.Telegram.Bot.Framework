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

namespace Telegram.Bot.Framework.Helper
{
    /// <summary>
    /// 这是 <see cref="bool"/> 的扩展方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="bool"/> 的扩展方法, 通常是以下的方式：
    /// </para>
    /// <code>
    /// public static void MethodName(this <see cref="bool"/> obj)
    /// </code>
    /// </remarks>
    public static class BoolHelper
    {
        /// <summary>
        /// 将传入值改变为 True
        /// </summary>
        /// <param name="value">传入布尔值</param>
        /// <returns>True</returns>
        public static bool True(this ref bool value)
        {
            value = true;
            return value;
        }

        /// <summary>
        /// 将传入值改变为 False
        /// </summary>
        /// <param name="value">传入布尔值</param>
        /// <returns>False</returns>
        public static bool False(this ref bool value)
        {
            value = false;
            return value;
        }
    }
}
