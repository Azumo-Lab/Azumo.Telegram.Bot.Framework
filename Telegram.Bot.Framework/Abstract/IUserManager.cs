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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// 获取我自己
        /// </summary>
        TelegramUser Me { get; }

        /// <summary>
        /// 获取管理员用户(Bot管理员)
        /// </summary>
        List<TelegramUser> Admin { get; }

        #region 查询用户
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        List<TelegramUser> GetUsers();

        /// <summary>
        /// 随机获取一个用户
        /// </summary>
        /// <returns></returns>
        TelegramUser RandomUser();

        /// <summary>
        /// 获取用户的UserScope
        /// </summary>
        /// <param name="telegramUser"></param>
        /// <returns></returns>
        IServiceScope GetUserScope(TelegramUser telegramUser);

        /// <summary>
        /// 获取用户的UserScope
        /// </summary>
        /// <param name="telegramUser"></param>
        /// <returns></returns>
        IServiceScope GetUserScope(string Username);

        /// <summary>
        /// 获取用户的UserScope
        /// </summary>
        /// <param name="telegramUser"></param>
        /// <returns></returns>
        IServiceScope GetUserScope(long ChatID);

        /// <summary>
        /// 通过用户名寻找用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        TelegramUser FindUserByUserName(string UserName);

        /// <summary>
        /// 根据用户的名字寻找用户
        /// </summary>
        /// <param name="FristName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        List<TelegramUser> FindUserByName(string FristName, string LastName);

        /// <summary>
        /// 通过用户ID寻找用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        TelegramUser FindUserByID(long ID);

        /// <summary>
        /// 通过ChatID寻找用户
        /// </summary>
        /// <param name="ChatID"></param>
        /// <returns></returns>
        TelegramUser FindUserByChatID(long ChatID);
        #endregion

        #region 删除用户
        /// <summary>
        /// 移除用户(再次访问后相当于新用户)
        /// </summary>
        /// <param name="user"></param>
        void RemoveUser(TelegramUser user);

        /// <summary>
        /// 根据ID移除用户
        /// </summary>
        /// <param name="ID"></param>
        void RemoveUser(long ID);
        #endregion

        #region 增加用户
        /// <summary>
        /// 注册一个用户
        /// </summary>
        /// <param name="user"></param>
        void RegisterUser(TelegramUser user);

        /// <summary>
        /// 注册一个用户
        /// </summary>
        /// <param name="user"></param>
        void RegisterUser(User user, long ChatID);
        #endregion

        #region 修改用户
        /// <summary>
        /// 屏蔽用户(用户访问Bot，Bot无响应)
        /// </summary>
        /// <param name="user"></param>
        void Block(TelegramUser user);

        /// <summary>
        /// 解除屏蔽
        /// </summary>
        /// <param name="user"></param>
        void Restore(TelegramUser user);
        #endregion
    }
}
