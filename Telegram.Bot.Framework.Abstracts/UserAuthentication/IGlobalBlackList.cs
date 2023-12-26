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

using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.UserAuthentication
{
    /// <summary>
    /// 全局屏蔽接口
    /// </summary>
    /// <remarks>
    /// 用于对用户进行全局屏蔽，非全局屏蔽的用户，会创建 <see cref="TelegramUserChatContext"/> 类，耗费额外的资源，
    /// 对于全局屏蔽用户的处理，则是在最初的 <see cref="TelegramUserChatContext"/> 创建时进行屏蔽。
    /// </remarks>
    public interface IGlobalBlackList
    {
        /// <summary>
        /// 在加载屏蔽列表时的事件
        /// </summary>
        /// <remarks>
        /// 这个事件是在加载屏蔽列表时发生，可以交由外部进行处理。读取数据库中或者文件中的屏蔽列表数据。
        /// </remarks>
        public event EventHandler<UserIDArgs>? OnLoadUserID;

        /// <summary>
        /// 屏蔽列表保存时的事件
        /// </summary>
        /// <remarks>
        /// 这个事件时在保存列表时候发生，可以交由外部进行保存处理，写入文件中或者数据库中。
        /// </remarks>
        public event EventHandler<UserIDArgs>? OnSaveUserID;

        /// <summary>
        /// 向全局屏蔽列表中添加一个屏蔽用户
        /// </summary>
        /// <param name="userID">用户的ID</param>
        public void Add(long userID);

        /// <summary>
        /// 从屏蔽列表中移除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        public void Remove(long userID);

        /// <summary>
        /// 验证用户是否是屏蔽用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>用户是否被屏蔽</returns>
        public bool Verify(long userID);

        /// <summary>
        /// 获取屏蔽列表
        /// </summary>
        /// <returns>屏蔽列表</returns>
        public List<long> GetList();

        /// <summary>
        /// 加载屏蔽列表
        /// </summary>
        public void Init();
    }

    /// <summary>
    /// 加载屏蔽列表参数
    /// </summary>
    /// <remarks>
    /// 这个参数用来加载屏蔽列表，由外部处理，向列表中添加数据
    /// </remarks>
    public class UserIDArgs : EventArgs
    {
        /// <summary>
        /// 从外部处理写入的屏蔽数据
        /// </summary>
        public List<long> UserIDs { get; } = [];
    }
}
