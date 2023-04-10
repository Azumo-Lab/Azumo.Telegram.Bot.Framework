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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public static class List_ExtensionMethod
    {
        public static T Get<T>(this List<T> list, int index, T defVal = default)
        {
            if (index < 0) return defVal;
            if (list.Count > index) return list[index];
            return defVal;
        }

        public static T GetAndRemove<T>(this List<T> list, int index = 0, T defVal = default)
        {
            if (index < 0) return defVal;
            if (list.Count > index)
            {
                try
                {
                    return list[index];
                }
                finally
                {
                    list.RemoveAt(index);
                }
            }
            return defVal;
        }
    }
}
