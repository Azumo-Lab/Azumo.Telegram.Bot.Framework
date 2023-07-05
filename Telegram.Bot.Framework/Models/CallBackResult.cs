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

namespace Telegram.Bot.Framework.Models
{
    /// <summary>
    /// 回调函数的执行结果
    /// </summary>
    public struct CallBackResult
    {
        /// <summary>
        /// 执行结果（是否成功/失败）
        /// </summary>
        public bool Success { get; internal set; }

        /// <summary>
        /// 执行的返回值
        /// </summary>
        public object Result { get; internal set; }

        /// <summary>
        /// 执行的错误信息
        /// </summary>
        public Exception Exception { get; internal set; }
    }
}
