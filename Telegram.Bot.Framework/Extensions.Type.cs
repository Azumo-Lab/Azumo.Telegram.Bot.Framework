//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System.Reflection;

namespace Telegram.Bot.Framework;
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<Type> GetAllSameType(this Type type) =>
        AllTypes.Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeType"></param>
    /// <returns></returns>
    public static List<(Type classType, Attribute[] attributes)> GetTypesWithAttribute(this Type attributeType) =>
        AllTypes.Where(classType => Attribute.IsDefined(classType, attributeType))
            .Select(classType => (classType, Attribute.GetCustomAttributes(classType, attributeType)!))
            .ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="AttributeType"></typeparam>
    /// <returns></returns>
    public static List<(Type classType, Attribute[] attributes)> GetTypesWithAttribute<AttributeType>() where AttributeType : Attribute =>
         typeof(AttributeType).GetTypesWithAttribute();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="AttributeType"></typeparam>
    /// <param name="type"></param>
    /// <param name="bindingFlags"></param>
    /// <returns></returns>
    public static List<(MethodInfo methodInfo, Attribute attribute)> GetMethodsWithAttribute<AttributeType>(this Type type, BindingFlags bindingFlags) where AttributeType : Attribute =>
        type.GetMethods(bindingFlags)
            .Where(methodInfo => Attribute.IsDefined(methodInfo, typeof(AttributeType)))
            .Select(methodInfo => (methodInfo, Attribute.GetCustomAttribute(methodInfo, typeof(AttributeType))!))
            .ToList();
}
