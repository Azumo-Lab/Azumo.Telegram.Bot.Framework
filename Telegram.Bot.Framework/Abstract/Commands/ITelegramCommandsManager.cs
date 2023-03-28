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
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Commands
{
    /// <summary>
    /// Command 指令管理器
    /// </summary>
    public interface ITelegramCommandsManager
    {
        #region 改
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITelegramCommandsChangeSession ChangeTelegramCommands();
        #endregion

        #region 查
        /// <summary>
        /// 系统中是否具有此Command
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public bool ContainsCommand(string commandName);

        /// <summary>
        /// 查询指定的指令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public BotCommand GetBotCommand(string commandName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BotCommand> GetBotCommands();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botCommandScope"></param>
        /// <returns></returns>
        public List<BotCommand> GetBotCommands(BotCommandScope botCommandScope);

        #endregion
    }
}
