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

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// 执行管理
    /// </summary>
    internal interface IProcessManager
    {
        /// <summary>
        /// 是否正在运行
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>True是/False否</returns>
        bool IsRunning(string ID);

        /// <summary>
        /// 停止执行
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>是否成功</returns>
        bool Stop(string ID);

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="EXEPath">可执行文件路径</param>
        /// <returns>是否成功</returns>
        bool Start(string ID, string EXEPath);
    }
}
