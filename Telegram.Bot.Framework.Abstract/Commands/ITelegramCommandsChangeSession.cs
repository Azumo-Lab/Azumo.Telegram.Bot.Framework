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
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITelegramCommandsChangeSession : IDisposable
    {
        #region 应用更改
        /// <summary>
        /// 应用更改
        /// </summary>
        public Task ApplyChanges();
        #endregion

        #region 增
        /// <summary>
        /// 注册指令，将指令注册到Telegram
        /// </summary>
        public void RegisterCommand(string commandName, string commandInfo, BotCommandScope botCommandScope = default!);
        #endregion

        #region 删
        /// <summary>
        /// 删除一个Command
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public void RemoveCommand(string commandName);
        #endregion

        #region 改
        /// <summary>
        /// 从删除的当中恢复一个Command
        /// </summary>
        /// <param name="commandName"></param>
        public void RestoreCommand(string commandName);

        public void ChangeScope(string commandName, BotCommandScope botCommandScope);
        #endregion
    }
}
