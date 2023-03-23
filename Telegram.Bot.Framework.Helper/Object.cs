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

namespace Telegram.Bot.Framework.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectHelper
    {
        #region 抛出异常
        public static void ThrowIfNull(this object obj)
        {
            if (obj.IsNull())
                throw new ArgumentNullException(nameof(obj));
        }

        #endregion

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool HasAllNull(params object[] objs)
        {
            return !objs.Where(o => !o.IsNull()).Any();
        }

        public static List<Type> GetSameType(this object obj)
        {
            Type? baseType = null;

            if (obj.IsNull())
                return Array.Empty<Type>().ToList();
            if (obj is Type type)
                baseType = type;
            else
                baseType = obj.GetType();

            List<Type> allTypes = GetAllTypes();
            allTypes = allTypes.Where(x =>
            {
                return baseType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface;
            }).ToList();
            return allTypes;
        }

        public static List<Type> GetAllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }
    }
}
