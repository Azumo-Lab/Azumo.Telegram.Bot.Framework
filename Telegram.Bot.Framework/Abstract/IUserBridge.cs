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

namespace Telegram.Bot.Framework.Abstract
{
    /// <summary>
    /// 用户桥接口
    /// 帮助用户间互通互动的接口，
    /// 可以使用该接口来做到实时通信
    /// </summary>
    public interface IUserBridge : IDisposable
    {
        /// <summary>
        /// 是否已关闭
        /// </summary>
        bool IsDiscard { get; }

        /// <summary>
        /// 目标用户
        /// </summary>
        TelegramUser TargetUser { get; }

        #region 各类事件
        public delegate void OnCreateHandle();
        public delegate void OnCloseHandle();

        /// <summary>
        /// 建立时的事件
        /// </summary>
        public event OnCreateHandle OnCreate;
        /// <summary>
        /// 销毁时的事件
        /// </summary>
        public event OnCloseHandle OnClose;
        #endregion

        /// <summary>
        /// 连接建立
        /// </summary>
        public void Connect();

        /// <summary>
        /// 连接关闭
        /// </summary>
        public void Disconnect();

        /// <summary>
        /// 向目标用户发送一条文本消息
        /// </summary>
        /// <param name="Message"></param>
        public void Send(string Message);
    }
}
