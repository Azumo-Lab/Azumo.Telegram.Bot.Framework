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

namespace Telegram.Bot.Framework.InternalFramework.FrameworkHelper
{
    /// <summary>
    /// 
    /// </summary>
    internal class TypesHelper
    {
        private readonly static List<Type> AllTypes;
        private readonly static Dictionary<string, List<Type>> Dic_AllTypes = new();

        static TypesHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        public static List<Type> GetTypes<T>()
        {
            Type BaseType = typeof(T);
            if (!Dic_AllTypes.ContainsKey(BaseType.FullName))
                Dic_AllTypes.Add(BaseType.FullName, AllTypes.Where(x => BaseType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface).ToList());
            return Dic_AllTypes[BaseType.FullName];
        }

        public static T GetAttr<T>(MemberInfo info) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(info, typeof(T));

        public static T GetAttr<T>(ParameterInfo info) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(info, typeof(T));
    }
}
