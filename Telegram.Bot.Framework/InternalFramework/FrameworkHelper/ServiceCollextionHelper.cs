﻿//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.InternalFramework.FrameworkHelper
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ServiceCollextionHelper
    {
        private static List<Type> AllTypes;
        static ServiceCollextionHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        public static List<Type> GetAllTypes()
        {
            return AllTypes;
        }

        public static List<Type> FilterBaseType(Type BaseType)
        {
            return AllTypes.Where(x => BaseType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface).ToList();
        }
    }
}