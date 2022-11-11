//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
    /// 
    /// </summary>
    public interface IBotTelegramEvent
    {
        /// <summary>
        /// 被邀请
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnInvited(TelegramContext context);

        /// <summary>
        /// 被踢
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnKicked(TelegramContext context);

        /// <summary>
        /// 离开群组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnLeft(TelegramContext context);

        /// <summary>
        /// 创建聊天
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnCreator(TelegramContext context);

        /// <summary>
        /// 成为管理员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnBeAdmin(TelegramContext context);
    }
}
