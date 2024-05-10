﻿//  <Telegram.Bot.Framework>
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

using System.Collections.Generic;

namespace Telegram.Bot.Framework.Core.Controller
{
    /// <summary>
    /// 指令管理器
    /// </summary>
    internal interface ICommandManager
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public IExecutor? GetExecutor(TelegramUserContext userContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executor"></param>
        public void AddExecutor(IExecutor executor);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IExecutor> GetExecutorList();
    }
}