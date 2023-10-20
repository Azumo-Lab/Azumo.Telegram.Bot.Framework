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

namespace Telegram.Bot.Framework.Helpers
{
    internal static class ReflectionHelper
    {
        public static IReadOnlyList<Type> AllTypes { get; }

        static ReflectionHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }
        public static List<Type> FindTypeOf(this object obj)
        {
            Type type = obj is Type newType ? newType : obj.GetType();
            return AllTypes.Where(type.IsAssignableFrom).Where(x => !x.IsInterface && !x.IsAbstract).ToList();
        }
    }
}
