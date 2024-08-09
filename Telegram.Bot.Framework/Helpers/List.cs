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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// 自动释放列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListDisposable<T> : List<T>, IDisposable where T : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            foreach (var item in this)
                item.Dispose();
        }
    }

    /// <summary>
    /// 异步自动释放列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListAsyncDisposable<T> : List<T>, IAsyncDisposable where T : IAsyncDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            foreach (var item in this)
                await item.DisposeAsync();
        }
    }
}
