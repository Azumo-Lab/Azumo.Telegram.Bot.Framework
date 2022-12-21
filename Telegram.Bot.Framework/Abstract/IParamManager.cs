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
    /// 帮助创建参数
    /// </summary>
    internal interface IParamManager
    {
        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> ReadParam(TelegramContext context, IServiceProvider OneTimeServiceProvider);

        /// <summary>
        /// 取消读取参数
        /// </summary>
        void Cancel();

        /// <summary>
        /// 获取Command的名称
        /// </summary>
        /// <returns></returns>
        string GetCommand();

        /// <summary>
        /// 获取读取过后的参数
        /// </summary>
        /// <returns></returns>
        object[] GetParam();

        /// <summary>
        /// 是否处于读取参数的模式
        /// </summary>
        /// <returns></returns>
        bool IsReadParam();
    }
}
