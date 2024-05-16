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

using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Authentication
{
    /// <summary>
    /// 认证相关的接口
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// 初始化角色信息
        /// </summary>
        /// <returns></returns>
        public Task InitRoles();

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        public string[] GetRoles();

        /// <summary>
        /// 设定角色信息
        /// </summary>
        /// <param name="roles"></param>
        public void SetRoles(params string[] roles);

        /// <summary>
        /// 移除角色信息
        /// </summary>
        /// <param name="roles"></param>
        public void RemoveRoles(params string[] roles);

        /// <summary>
        /// 清空角色信息
        /// </summary>
        public void ClearRoles();

        /// <summary>
        /// 屏蔽这个用户角色
        /// </summary>
        public Task BanThisChat();

        /// <summary>
        /// 变更更改
        /// </summary>
        /// <returns></returns>
        public Task ApplyChangesAsync();
    }
}
