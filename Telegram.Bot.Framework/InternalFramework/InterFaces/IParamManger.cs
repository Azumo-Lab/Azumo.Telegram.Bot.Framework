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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Telegram.Bot.Framework.InternalFramework.InterFaces
{
    internal interface IParamManger
    {
        /// <summary>
        /// 是否处于读取参数的模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsReadParam(TelegramContext context);

        /// <summary>
        /// 取消读取参数
        /// </summary>
        /// <param name="context"></param>
        void Cancel(TelegramContext context);

        /// <summary>
        /// 设置指令
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="context"></param>
        void SetCommand(string Command, TelegramContext context);

        /// <summary>
        /// 设置指令
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="context"></param>
        bool StartReadParam(TelegramContext context, IServiceProvider serviceProvider);

        /// <summary>
        /// 获取读取过后的参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object[] GetParam(TelegramContext context);
    }
}
