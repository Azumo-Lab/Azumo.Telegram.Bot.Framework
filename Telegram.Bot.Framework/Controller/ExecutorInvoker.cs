//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
//
//  Author: 牛奶

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Params;
using Telegram.Bot.Framework.Controller.Results;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class ExecutorInvoker : IExecutor
    {
        /// <summary>
        /// 指令方法信息
        /// </summary>
        protected MethodInfo MethodInfo { get; set; } = null!;

        /// <summary>
        /// 指令方法编译后的委托
        /// </summary>
        /// <remarks>
        /// <code>Func&lt;object, object?[]?, object?&gt;</code>
        /// 参数1：object 实例对象<br></br>
        /// 参数2：object?[]? 参数数组<br></br>
        /// 返回值：object? 方法返回值<br></br>
        /// </remarks>
        protected Func<object, object?[]?, object?> InvokerFunc { get; set; } = null!;

        /// <summary>
        /// 类和方法的 <see cref="Attribute"/>
        /// </summary>
        public Attribute[] Attributes { get; protected set; } = null!;

        /// <summary>
        /// 方法参数的 <see cref="IGetParam"/> 接口列表
        /// </summary>
        public IReadOnlyList<IGetParam> Parameters => ParametersList;

        /// <summary>
        /// 
        /// </summary>
        protected ObjectFactory ObjectFactory { get; private set; } = null!;

        /// <summary>
        /// 缓存
        /// </summary>
        public Dictionary<string, object> Cache { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 可修改的参数列表
        /// </summary>
        protected List<IGetParam> ParametersList { get; } = new List<IGetParam>();

        /// <summary>
        /// <see cref="Delegate"/> 类型的实例对象
        /// </summary>
        protected object? Target { get; set; }

        /// <summary>
        /// 解析指令方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="target"></param>
        public virtual void Analyze(MethodInfo methodInfo, object? target = null)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            InvokerFunc = BuildTaskFunc(MethodInfo);
            Target = target;

            var parameters = MethodInfo.GetParameters();
            foreach (var parameterInfo in parameters)
            {
                var getParam = CreateIGetParam(parameterInfo);
                ParametersList.Add(getParam);
            }

            var attributes = new List<Attribute>();
            attributes.AddRange(TypeDescriptor.GetAttributes(MethodInfo).Cast<Attribute>());
            attributes.AddRange(Attribute.GetCustomAttributes(MethodInfo));
            if (MethodInfo.DeclaringType != null)
            {
                attributes.AddRange(TypeDescriptor.GetAttributes(MethodInfo.DeclaringType).Cast<Attribute>());
                attributes.AddRange(Attribute.GetCustomAttributes(MethodInfo.DeclaringType));
            }
            Attributes = attributes.ToArray();

            if (!(MethodInfo.IsStatic && MethodInfo.DeclaringType == null))
            {
                ObjectFactory = ActivatorUtilities.CreateFactory(MethodInfo.DeclaringType!, Array.Empty<Type>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        internal static async Task ConvertTo(Task task) =>
            await task;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static IGetParam CreateIGetParam(ParameterInfo parameterInfo)
        {
            Type? iGetParamType = null;
            // 从参数上获取 ParamAttribute 标签
            var paramAttribute = Attribute.GetCustomAttribute(parameterInfo, typeof(ParamAttribute)) as ParamAttribute;
            if (paramAttribute != null) // 能获取到就使用获取到的类型
                if (paramAttribute.IGetParmType != null)
                    iGetParamType = paramAttribute.IGetParmType;

            // 使用默认逻辑
            if (iGetParamType == null)
            {
                var paramval = Extensions.IGetParamTypeList
                    .Where(y => y.ForType.ForType.FullName == parameterInfo.ParameterType.FullName)
                    .Select(y => y.classType)
                    .FirstOrDefault() ?? typeof(NullParam);
                iGetParamType = paramval;
            }

            // 获取构造函数
            ConstructorInfo? constructorInfo;
            if ((constructorInfo = iGetParamType.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault()) == null)
                throw new Exception("没有找到对应的初始化方法");

            // 判断是否有参数
            if (constructorInfo.GetParameters().Length != 0)
                throw new Exception("无法生成带有参数的类");

            // 实例化
            var result = constructorInfo.Invoke(Array.Empty<object>());
            if (result is IGetParam getParam)
                getParam.ParamAttribute = paramAttribute;
            else if (result == null)
                throw new NullReferenceException($"类型：{iGetParamType.FullName} 无法实例化");
            else
                throw new Exception($"类型：{iGetParamType.FullName} 未实现接口 {nameof(IGetParam)}");
            return getParam;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Expression[] BuildParamters(ParameterInfo[] parameterInfos, ParameterExpression parameter)
        {
            var paramList = new Expression[parameterInfos.Length];
            for (var i = 0; i < paramList.Length; i++)
                paramList[i] = Expression.Convert(Expression.ArrayIndex(parameter, Expression.Constant(i)), parameterInfos[i].ParameterType);
            return paramList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        private static Expression ReturnType(MethodInfo methodInfo, Expression methodResult)
        {
            Expression returnExpression;
            var returnType = methodInfo.ReturnType;
            if (returnType == typeof(void))
            {
                returnExpression = Expression.Constant(null, typeof(object));
            }
            else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var list = new List<Expression>();
                static Expression CreateExpression(Type type)
                {
                    // Task<Task<string>>
                    // 把 Task 类型 转换为 Task<Task<string>>，并调用Result
                    var TaskAwait = Expression.Parameter(typeof(Task), "TaskAwait");
                    var TaskConvert = Expression.Convert(TaskAwait, type);
                    // Task<string>
                    var result = Expression.Property(TaskConvert, nameof(Task<object>.Result));
                    Expression resuFunc = Expression.Lambda<Func<Task, object?>>(result, TaskAwait);
                    return resuFunc;
                }

                var convertTo = typeof(ExecutorInvoker).GetMethod(nameof(ConvertTo), BindingFlags.Static | BindingFlags.NonPublic)!;

                // 呼叫ConvertTo方法
                var taskMethodResult = Expression.Call(null, convertTo, methodResult);
                Expression convertValue = Expression.Invoke(CreateExpression(returnType), methodResult);
                // 呼叫ConvertTo方法
                var tt = Expression.Block(taskMethodResult, convertValue);

                list.Add(tt);

                var childType = returnType.GetGenericArguments();
                var expression = convertValue;
            NEXT:
                foreach (var item in childType)// Task<string>
                {
                    // 泛型依然是Task<>类型
                    if (item.IsGenericType && item.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        // 将Object类型转换为 内部的 泛型
                        var taskValue = Expression.Convert(expression, item);
                        // 呼叫ConvertTo方法，并将新参数传入
                        var aa = Expression.Call(null, convertTo, taskValue);
                        // 返回新的泛型类型
                        expression = Expression.Invoke(CreateExpression(item), taskValue);
                        var cc = Expression.Block(aa, taskValue);

                        list.Add(cc);
                        childType = item.GetGenericArguments();
                        goto NEXT;
                    }
                }

                list.Add(expression);

                var methodReturnInstance = Expression.Parameter(returnType);
                returnExpression = Expression.Invoke(Expression.Lambda(Expression.Block(list), methodReturnInstance), methodResult);
            }
            else
            {
                returnExpression = Expression.Convert(methodResult, typeof(object));
            }
            return returnExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Func<object, object?[]?, object?> BuildTaskFunc(MethodInfo methodInfo)
        {
            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var paramArrays = Expression.Parameter(typeof(object[]), "param");
            var itemParamList = BuildParamters(methodInfo.GetParameters(), paramArrays);

            // 实例类型转换
            Expression? instanceConvert = null;
            if (instanceType != null && !isStatic)
                instanceConvert = Expression.Convert(instance, instanceType);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            // 呼叫函数
            var methodResultObject = Expression.Call(instanceConvert, methodInfo, itemParamList);

            // 返回值处理
            var returnResult = ReturnType(methodInfo, methodResultObject);

            // 创建函数
            var result = Expression.Lambda<Func<object, object?[]?, object?>>(returnResult, instance, paramArrays).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public abstract Task<(ControllerResult, IActionResult?)> ActionExecute(TelegramActionContext telegramActionContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public abstract Task<object?> Invoke(TelegramActionContext telegramActionContext);
    }
}
