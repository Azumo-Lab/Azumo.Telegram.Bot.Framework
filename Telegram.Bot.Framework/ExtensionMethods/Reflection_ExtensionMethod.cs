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
    public static class Reflection_ExtensionMethod
    {
        private static List<Type> __AllTypes = new List<Type>();

        static Reflection_ExtensionMethod()
        {
            __AllTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Type> GetSameTypes(this object obj)
        {
            obj.ThrowIfNull();

            Type baseType = null;
            if (obj is Type t)
                baseType = t;
            else
                baseType = obj.GetType();

            return __AllTypes.Where(x =>
            {
                return !x.IsAbstract && !x.IsInterface && baseType.IsAssignableFrom(x);
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllTypes()
        {
            return __AllTypes;
        }
    }
}
