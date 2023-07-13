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

namespace Telegram.Bot.Framework.Models.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum ChatScopeEnum
    {
        /// <summary>
        /// 所有的一对一聊天（私聊机器人）
        /// </summary>
        AllPrivateChat = 1 << 0,

        /// <summary>
        /// 所有的频道
        /// </summary>
        AllChannel = 1 << 1,

        /// <summary>
        /// 所有的群组
        /// </summary>
        AllGroup = 1 << 2,

        /// <summary>
        /// 所有的小组成员（全部小组的，全部成员）
        /// </summary>
        AllGroupAllMembers = 1 << 3,

        /// <summary>
        /// 所有小组的管理员（全部小组的，全部管理员）
        /// </summary>
        AllGroupAllAdmins = 1 << 4,

        /// <summary>
        /// 指定的一对一聊天（私聊机器人）
        /// </summary>
        PrivateChat = 1 << 5,

        /// <summary>
        /// 指定的频道
        /// </summary>
        Channel = 1 << 6,

        /// <summary>
        /// 指定的群组
        /// </summary>
        Group = 1 << 7,

        /// <summary>
        /// 指定的小组成员（指定小组的，全部成员）
        /// </summary>
        GroupAllMembers = 1 << 8,

        /// <summary>
        /// 指定小组的管理员（指定小组的，全部管理员）
        /// </summary>
        GroupAllAdmins = 1 << 9,

        /// <summary>
        /// 指定的小组，指定的成员（指定小组的，指定成员）
        /// </summary>
        GroupMembers = 1 << 10,

        /// <summary>
        /// 指定小组，指定管理员（指定小组的，指定管理员）
        /// </summary>
        GroupAdmins = 1 << 11,

        /// <summary>
        /// 无任何限制
        /// </summary>
        All = 1 << 12,
    }
}
