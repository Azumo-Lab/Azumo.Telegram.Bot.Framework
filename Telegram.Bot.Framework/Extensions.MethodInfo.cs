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

using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    public static Func<IServiceProvider, object[], object> BuildFunc(this MethodInfo methodInfo)
    {
        // 创建对象创建工厂
        var instanceType = methodInfo.DeclaringType;
        var isStatic = methodInfo.IsStatic;

        if (!isStatic && instanceType == null)
            throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

        // 创建对象工厂
        Expression<Func<IServiceProvider, object>> objectFactoryFunc;
        if (!isStatic)
        {
            var objectFactory = ActivatorUtilities.CreateFactory(instanceType!, []);
            RuntimeHelpers.PrepareDelegate(objectFactory);

            objectFactoryFunc = (service) => objectFactory(service, Array.Empty<object>());
        }
        else
        {
            objectFactoryFunc = (service) => null!;
        }
            
        // 参数
        var parameters = methodInfo.GetParameters();
        var serviceProvider = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
        var param = Expression.Parameter(typeof(object[]), "param");

        var paramList = new Expression[parameters.Length];

        for (var i = 0; i < paramList.Length; i++)
            paramList[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameters[i].ParameterType);

        // 调用方法
        RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

        var objectFactoryInvoke = Expression.Invoke(objectFactoryFunc, serviceProvider);
        var method = Expression.Call(methodInfo.IsStatic ? objectFactoryInvoke : Expression.Convert(objectFactoryInvoke, methodInfo.DeclaringType!), methodInfo, paramList);
        Expression func;
        if (methodInfo.ReturnType.FullName == typeof(void).FullName)
        {
            Expression<Func<object>> expression = () => null!;
            func = Expression.Block(method, expression);
        }
        else
        {
            func = Expression.Block(method);
        }

        var result = Expression.Lambda<Func<IServiceProvider, object[], object>>(func, serviceProvider, param).Compile();
        RuntimeHelpers.PrepareDelegate(result);
        return result;
    }
}
