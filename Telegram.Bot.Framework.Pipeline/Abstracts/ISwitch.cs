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

namespace Telegram.Bot.Framework.Pipeline.Abstracts
{
    /// <summary>
    /// 相当于Switch关键字
    /// </summary>
    /// <remarks>
    /// 这是一个类似Switch关键字的一个接口，可以使用添加新实现的方式，而不是修改代码的方式进行功能扩展
    /// </remarks>
    public interface ISwitch<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public T SwitchKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchTo();
    }
}
