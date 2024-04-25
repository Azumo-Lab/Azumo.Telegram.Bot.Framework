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
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Reflection;

namespace Telegram.Bot.Framework;
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    public static Func<object, object[], object> BuildFunc(this MethodInfo methodInfo)
    {
        


        var parameters = methodInfo.GetParameters();

        var instance = Expression.Parameter(typeof(object), "instance");
        var param = Expression.Parameter(typeof(object[]), "param");

        var paramList = new Expression[parameters.Length];

        for (var i = 0; i < paramList.Length; i++)
            paramList[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameters[i].ParameterType);

        var method = Expression.Call(methodInfo.IsStatic ? null : instance, methodInfo, paramList);
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
        return Expression.Lambda<Func<object, object[], object>>(func, instance, param).Compile();
    }
}
