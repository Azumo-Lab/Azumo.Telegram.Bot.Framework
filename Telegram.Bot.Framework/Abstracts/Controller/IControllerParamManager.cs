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

using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    /// <summary>
    /// 参数管理控制器
    /// </summary>
    public interface IControllerParamManager
    {
        /// <summary>
        /// 参数的指令
        /// </summary>
        internal BotCommand BotCommand { get; set; }

        /// <summary>
        /// 指示第几个参数
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        /// 参数的状态
        /// </summary>
        internal ParamStauts ParamStauts { get; set; }

        /// <summary>
        /// 获得所有的参数
        /// </summary>
        /// <returns></returns>
        public object[] GetObjects();

        /// <summary>
        /// 添加结果
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(object obj);

        /// <summary>
        /// 清除所有的数据
        /// </summary>
        public void Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    internal enum ParamStauts
    {
        Read,
        Write
    }
}
