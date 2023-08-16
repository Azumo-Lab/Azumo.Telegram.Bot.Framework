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

using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.User
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// 用户的总数
        /// </summary>
        /// <remarks>
        /// 用于显示当前用户的总数，当前用户的总数计算是24小时内向Bot发送过任意消息的用户
        /// </remarks>
        public int UserCount { get; }

        /// <summary>
        /// 创建一个用户 <see cref="IChat"/>
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="serviceScope"></param>
        /// <returns>返回一个 <see cref="IChat"/> 实例 </returns>
        public IChat CreateIChat(ITelegramBotClient botClient, Update update, IServiceScope serviceScope);

        /// <summary>
        /// 更新在线用户数量
        /// </summary>
        public void UpdateUserCount();
    }
}
