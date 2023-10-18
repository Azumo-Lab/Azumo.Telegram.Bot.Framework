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
    /// 
    /// </summary>
    public interface IControllerParamManager
    {
        /// <summary>
        /// 
        /// </summary>
        internal BotCommand BotCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal ParamStauts ParamStauts { get; set; }

        /// <summary>
        /// 获得所有的参数
        /// </summary>
        /// <returns></returns>
        public object[] GetObjects();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(object obj);

        /// <summary>
        /// 清除所有的数据
        /// </summary>
        public void Clear();
    }

    internal enum ParamStauts
    {
        Read,
        Write
    }
}
