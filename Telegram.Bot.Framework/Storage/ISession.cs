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

namespace Telegram.Bot.Framework.Storage
{
    /// <summary>
    /// 用户区域的存储接口
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// 存储ID
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// 存储数量
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// 添加新的键值对
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">值</param>
        public void Add(object key, object value);

        /// <summary>
        /// 移除指定的数据
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(object key);

        /// <summary>
        /// 清除所有的数据
        /// </summary>
        public void Clear();

        /// <summary>
        /// 获取指定的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>获得指定的数据</returns>
        public object Get(object key);

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要添加或更新的值</param>
        public void AddOrUpdate(object key, object value);
    }
}
