//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

namespace Azumo.Pipeline.Abstracts
{
    /// <summary>
    /// 一条流水线
    /// </summary>
    /// <remarks>
    /// 一条流水线，泛型为流水线处理的数据类型
    /// </remarks>
    /// <typeparam name="T">流水线处理的数据类型</typeparam>
    public interface IPipeline<T>
    {
        /// <summary>
        /// 执行流水线
        /// </summary>
        /// <param name="obj">要进行处理的数据</param>
        /// <returns>返回处理后的数据（异步）</returns>
        public Task<T> Invoke(T obj);
    }
}
