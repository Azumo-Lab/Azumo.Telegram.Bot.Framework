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
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework
{
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
        public static Func<IServiceProvider, object[], object?> BuildFuncFactory(this MethodInfo methodInfo)
        {
            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 创建对象工厂
            Expression<Func<IServiceProvider, object?>> objectFactoryFunc;
            if (!isStatic)
            {
                var objectFactory = ActivatorUtilities.CreateFactory
#if NET8_0_OR_GREATER
                    (instanceType!, []);
#else
                    (instanceType!, Array.Empty<Type>());
#endif

                RuntimeHelpers.PrepareDelegate(objectFactory);

                objectFactoryFunc = (service) => objectFactory(service, Array.Empty<object>());
            }
            else
                objectFactoryFunc = (service) => null;

            // 参数
            var serviceProvider = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
            var param = Expression.Parameter(typeof(object[]), "param");
            var paramList = methodInfo.GetParameters().BuildParamters(param);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            var objectFactoryInvoke = Expression.Invoke(objectFactoryFunc, serviceProvider);
            Expression instance;
#if NET6_0_OR_GREATER
            instance = methodInfo.IsStatic ? objectFactoryInvoke : Expression.Convert(objectFactoryInvoke, methodInfo.DeclaringType!);
#else
            instance = methodInfo.IsStatic ?
                objectFactoryInvoke :
                (Expression)Expression.Convert(objectFactoryInvoke, methodInfo.DeclaringType);
#endif
            var method = Expression.Call(instance, methodInfo, paramList);
            Expression func;
            if (methodInfo.ReturnType.FullName == typeof(void).FullName)
            {
                Expression<Func<object?>> expression = () => null;
                func = Expression.Block(method, expression);
            }
            else
            {
                func = Expression.Block(method);
            }

            var result = Expression.Lambda<Func<IServiceProvider, object[], object?>>(func, serviceProvider, param).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        
        private readonly static Func<Task<object?>> NullResult = 
            () => Task.FromResult<object?>(null);

        //private readonly static Func<object, Task<object?>> TaskResult = 
        //    (obj) =>
        //    {
        //        if (obj == null)
        //            return NullResult();
        //        if (obj is )
        //        {

        //        }
        //    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Func<object, object[], object?> BuildFunc(this MethodInfo methodInfo)
        {
            RuntimeHelpers.PrepareDelegate(NullResult);

            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var param = Expression.Parameter(typeof(object[]), "param");
            var paramList = methodInfo.GetParameters().BuildParamters(param);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            var method = Expression.Call(instance, methodInfo, paramList);
            Expression func;
            if (methodInfo.ReturnType.FullName == typeof(void).FullName)
            {
                Expression<Func<Task<object?>>> nullResult = () => Task.FromResult<object?>(null);
                func = Expression.Block(method, nullResult);
            }
            else
            {
                func = Expression.Block(method);
            }

            var result = Expression.Lambda<Func<object, object[], object?>>(func, null!, param).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Func<object, object[], Task<object?>> BuildTaskFunc(this MethodInfo methodInfo)
        {
            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var param = Expression.Parameter(typeof(object[]), "param");
            var paramList = methodInfo.GetParameters().BuildParamters(param);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            var method = Expression.Call(instance, methodInfo, paramList);

            Expression func;
            var MethodResultType = methodInfo.ReturnType;
            Type GenericType;
            // Void 返回值
            if (MethodResultType == typeof(void))
            {
                Expression<Func<object?>> expression = () => null;
                func = Expression.Block(method, expression);
            }
            // 带泛型的返回值
            else if (MethodResultType.IsGenericType && (GenericType = MethodResultType.GetGenericTypeDefinition()) == typeof(Task<>))
            {
                foreach (var item in GenericType.GetGenericArguments())
                {
                    if (!(item.IsGenericType && item.GetGenericTypeDefinition() == typeof(Task<>)))
                    {
                        goto 创建函数;
                    }
                }

                var taskAwaitType = Expression.Parameter(MethodResultType, "taskAwaitType");

                var taskAwaiter = typeof(TaskAwaiter<>).MakeGenericType(MethodResultType.GetGenericArguments());
                var getAwaiter = Expression.Call(param, MethodResultType.GetMethod(nameof(Task.GetAwaiter))!);
                var getResult = Expression.Call(getAwaiter, taskAwaiter.GetMethod(nameof(TaskAwaiter.GetResult))!);

                MethodInfo MethodGetAwaiter;
                MethodInfo MethodGetResult;

                var processTaskType = MethodResultType;
            设置Task泛型:
                foreach (var item in processTaskType.GetGenericArguments())
                {
                    if (item.IsGenericType && item.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        foreach (var item2 in item.GetGenericArguments())
                        {
                            if (!(item.IsGenericType && item.GetGenericTypeDefinition() == typeof(Task<>)))
                            {

                            }
                        } 
                        MethodGetAwaiter = item.GetMethod(nameof(Task.GetAwaiter))!;
                        MethodGetResult = typeof(TaskAwaiter<>).MakeGenericType(item.GetGenericArguments()).GetMethod(nameof(TaskAwaiter.GetResult))!;

                        getAwaiter = Expression.Call(getResult, MethodGetAwaiter);
                        getResult = Expression.Call(getAwaiter, MethodGetResult);

                        processTaskType = item;
                        goto 设置Task泛型;
                    }
                }

                var MethodResultFunc = Expression.Lambda(getResult, param).Compile();

                RuntimeHelpers.PrepareDelegate(MethodResultFunc);
            }
            // Object 返回值
            else
            {
                
            }

        创建函数:

            var result = Expression.Lambda<Func<object, object[], Task<object?>>>(null!, null!, param).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Expression[] BuildParamters(this ParameterInfo[] parameterInfos, ParameterExpression parameter)
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
        /// <param name="parameterExpression"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Expression BuildInstance(this MethodInfo methodInfo, Expression parameterExpression)
        {
            Expression result;
            if (methodInfo.IsStatic)
                result = Expression.Constant(null, typeof(object));
            else
            {
                if (methodInfo.DeclaringType == null)
                    throw new Exception();

                result = Expression.Convert(parameterExpression, methodInfo.DeclaringType);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Expression<Func<IServiceProvider, object?>> BuildObjectFactory(this MethodInfo methodInfo)
        {
            if (methodInfo.IsStatic)
                return (service) => null;

            var instanceType = methodInfo.DeclaringType;
            if (instanceType == null)
                return (service) => null;

            var objectFactory =
#if NET8_0_OR_GREATER
                ActivatorUtilities.CreateFactory(instanceType, []);
#else
                ActivatorUtilities.CreateFactory(instanceType, Array.Empty<Type>());
#endif
            RuntimeHelpers.PrepareDelegate(objectFactory);
            Expression<Func<IServiceProvider, object?>> objectFactoryFunc = (service) => objectFactory(service, Array.Empty<object>());
            return objectFactoryFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Action<object, object[]> BuildAction(this MethodInfo methodInfo)
        {
            static Delegate buildDelegate(MethodInfo methodInfoArgs)
            {
                if (methodInfoArgs.ReturnType.FullName != typeof(void).FullName)
                    throw new Exception("The method must be void.");

                var instance = Expression.Parameter(typeof(object), "instance");
                var param = Expression.Parameter(typeof(object[]), "param");

                var method = Expression.Call(
                    methodInfoArgs.BuildInstance(instance),
                    methodInfoArgs,
                    methodInfoArgs.GetParameters().BuildParamters(param));

                var result = Expression.Lambda<Action<object, object[]>>(method, instance, param).Compile();

                return result;
            }
            return Build<Action<object, object[]>>(buildDelegate, methodInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Action<IServiceProvider, object[]> BuildActionFactory(this MethodInfo methodInfo)
        {
            static Delegate buildDelegate(MethodInfo methodInfoArgs)
            {
                if (methodInfoArgs.ReturnType.FullName != typeof(void).FullName)
                    throw new Exception("The method must be void.");

                var instance = Expression.Parameter(typeof(IServiceProvider), "instance");
                var param = Expression.Parameter(typeof(object[]), "param");

                var method = Expression.Call(
                    methodInfoArgs.BuildInstance(Expression.Invoke(methodInfoArgs.BuildObjectFactory(), instance)),
                    methodInfoArgs,
                    methodInfoArgs.GetParameters().BuildParamters(param));

                var result = Expression.Lambda<Action<object, object[]>>(method, instance, param).Compile();

                return result;
            }
            return Build<Action<IServiceProvider, object[]>>(buildDelegate, methodInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<object?>> NullFunc() => 
            () => null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        public static void PrepareMethod(this MethodInfo methodInfo) =>
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateMethod"></param>
        public static void PrepareDelegate(this Delegate delegateMethod) =>
            RuntimeHelpers.PrepareDelegate(delegateMethod);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Build<T>(Func<MethodInfo, Delegate> func, MethodInfo methodInfo) where T : Delegate
        {
            methodInfo.PrepareMethod();
            var delegateMethod = func.Invoke(methodInfo);
            delegateMethod.PrepareDelegate();
            return (delegateMethod as T) ?? throw new Exception();
        }
    }
}
