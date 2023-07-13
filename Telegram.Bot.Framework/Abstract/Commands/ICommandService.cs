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

namespace Telegram.Bot.Framework.Abstract.Commands
{
    /// <summary>
    /// 指令相关的服务
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// 是否有指令
        /// </summary>
        public bool HasCommand { get; }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <returns>返回指令</returns>
        public string GetCommand();

        /// <summary>
        /// 获取指令的执行方法
        /// </summary>
        /// <returns></returns>
        public Func<TelegramController, object[], Task> GetCommandFunc();

        /// <summary>
        /// 指令的管理
        /// </summary>
        public ICommandManager CommandManager { get; }
    }
}
