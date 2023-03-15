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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.Session
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class InternalSession : ISession, IDisposable
    {
        public string SessionID { get; } = Guid.NewGuid().ToString();

        private readonly Dictionary<object, byte[]> __InternalSessionCache = new();
        private bool __Disposed;

        public void Dispose()
        {
            __Disposed = true;
            __InternalSessionCache.Clear();
        }

        public byte[] Get(object sessionKey)
        {
            ThrowIfDispose();

            return __InternalSessionCache.TryGetValue(sessionKey, out byte[] result) ? result : default;
        }

        public void Remove(object sessionKey)
        {
            ThrowIfDispose();

            __InternalSessionCache.Remove(sessionKey);
        }

        public void Save(object sessionKey, byte[] data)
        {
            ThrowIfDispose();

            __InternalSessionCache.TryAdd(sessionKey, data);
        }

        private void ThrowIfDispose()
        {
            if (__Disposed)
                throw new Exception("Object is Disposed");
        }
    }
}
