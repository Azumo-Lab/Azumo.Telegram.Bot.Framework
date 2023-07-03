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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// 内部Session实现
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ISession))]
    internal sealed class InternalSession : ISession, IDisposable
    {
        /// <summary>
        /// 当前Session ID
        /// </summary>
        public string SessionID { get; } = Guid.NewGuid().ToString();

        private readonly Dictionary<object, byte[]> __InternalSessionCache = new();
        private bool __Disposed;

        /// <summary>
        /// 使用Key 获得相应的值
        /// </summary>
        /// <param name="sessionKey">要获取数据的Key</param>
        /// <returns>返回对应的数据，可以为NULL</returns>
        public byte[] Get(object sessionKey)
        {
            ThrowIfDispose();

            return __InternalSessionCache.TryGetValue(sessionKey, out byte[] result) ? result : default;
        }

        /// <summary>
        /// 删除指定Key的数据
        /// </summary>
        /// <param name="sessionKey">要删除数据的Key</param>
        public void Remove(object sessionKey)
        {
            ThrowIfDispose();

            __InternalSessionCache.Remove(sessionKey);
        }

        /// <summary>
        /// 保存对应Key的数据
        /// </summary>
        /// <param name="sessionKey">要保存数据的Key</param>
        /// <param name="data">要保存的数据</param>
        public void Save(object sessionKey, byte[] data)
        {
            ThrowIfDispose();

            if (__InternalSessionCache.ContainsKey(sessionKey))
            {
                __InternalSessionCache[sessionKey] = data;
            }
            else
            {
                __InternalSessionCache.Add(sessionKey, data);
            }
        }

        private void ThrowIfDispose()
        {
            if (__Disposed)
                throw new Exception($"{nameof(ISession)} is Disposed");
        }

        private void Dispose(bool disposing)
        {
            if (!__Disposed)
            {
                if (disposing)
                {
                    // 释放托管状态(托管对象)
                    __InternalSessionCache.Clear();
                }

                // 释放未托管的资源(未托管的对象)并重写终结器
                // 将大型字段设置为 null
                __Disposed = true;
            }
        }

        // // 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~InternalSession()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
