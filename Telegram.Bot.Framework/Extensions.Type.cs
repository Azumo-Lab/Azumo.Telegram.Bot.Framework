using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
