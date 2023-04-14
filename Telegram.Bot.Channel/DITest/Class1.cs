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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Telegram.Bot.Framework;

namespace Telegram.Bot.Channel.DITest
{
    public class Container
    {
        private string Guid = "[Guid(\"C6608E16-C627-4123-962D-F895235513C6\")]";

        private readonly Dictionary<Type, Delegate> NewInvoke = new ();
        public static void GetInstance()
        {
            try
            {
                // 获取方法信息
                MethodInfo methodInfo = typeof(Container).GetMethod("Test", BindingFlags.Instance | BindingFlags.Public);

                // 创建表达式树
                ParameterExpression containerParam = Expression.Parameter(typeof(Container), "container");
                ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

                var paramArray = methodInfo.GetParameters();
                List<Expression> expressionList = new List<Expression>();
                for (int i = 0; i < paramArray.Length; i++)
                {
                    var param = paramArray[i];
                    var e = Expression.Convert(Expression.ArrayIndex(argsParam, Expression.Constant(i)), param.ParameterType);
                    expressionList.Add(e);
                }

                MethodCallExpression call = Expression.Call(containerParam, methodInfo,
                    expressionList.ToArray());
                Expression<Action<Container, object[]>> lambda = Expression.Lambda<Action<Container, object[]>>(call, containerParam, argsParam);

                // 编译表达式树
                Action<Container, object[]> Action = lambda.Compile();

                RuntimeHelpers.PrepareDelegate(Action);

                // 执行方法
                Container container = new Container();
                object[] parameters = new object[] { "hello world", 5 } ;
                Action(container, parameters);
                Action(container, parameters);
                Action(container, parameters);
                Action(container, parameters);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void Test(string str, int ii)
        {
            Console.WriteLine(ii);
            Console.WriteLine(str);
            Console.WriteLine(Guid);
        }
    }

}
