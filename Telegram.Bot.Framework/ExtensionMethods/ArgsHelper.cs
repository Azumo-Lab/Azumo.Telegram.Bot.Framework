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

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArgsHelper
    {
        /// <summary>
        /// 获取参数， -Token "Token" -Proxy "http://127.0.0.1:7890/"
        /// </summary>
        /// <param name="args">参数列表</param>
        /// <param name="name">想要捕获的参数</param>
        /// <returns>返回的是想要的参数</returns>
        public static string GetArgs(this string[] args, string name)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower() == name.ToLower() || arg.ToUpper() == name.ToUpper())
                    return args.GetVal(i);
            }
            return string.Empty;
        }
    }
}
