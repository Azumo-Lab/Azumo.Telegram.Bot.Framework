//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
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
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 使用自定义的参数获取方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ParamMakerFor : Attribute
    {
        /// <summary>
        /// 参数获取类
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="type">要使用的类(实现IParamMaker)</param>
        public ParamMakerFor(Type type)
        {
            Type = type;
        }
    }
}
