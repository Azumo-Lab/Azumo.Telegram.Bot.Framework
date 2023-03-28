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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.InternalImplementation.Controller
{
    /// <summary>
    /// 执行的基类
    /// </summary>
    internal abstract class InvokeBase
    {
        /// <summary>
        /// 执行一个指令
        /// </summary>
        /// <param name="commandInfo">指令信息</param>
        /// <param name="telegramController">控制器</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        protected async Task CommandInvoke(CommandInfo commandInfo, TelegramController telegramController, params object[] param)
        {
            if ((commandInfo?.CommandMethod)!.IsNull() || telegramController.IsNull())
                return;

            Task task = param.IsEmpty()
                ? commandInfo!.CommandMethod!.Invoke(telegramController, Array.Empty<object>()) as Task
                : commandInfo!.CommandMethod!.Invoke(telegramController, param) as Task;
            if (task != null)
                await task;
        }
    }

    /// <summary>
    /// 创建委托
    /// </summary>
    internal static class DelegateHelper
    {
        #region 初始化
        private static readonly Dictionary<int, Type> ActionTypes = new();
        private static readonly Dictionary<int, Type> FuncTypes = new();
        static DelegateHelper()
        {
            ActionTypes.Add(0, typeof(Action));
            ActionTypes.Add(1, typeof(Action<>));
            ActionTypes.Add(2, typeof(Action<,>));
            ActionTypes.Add(3, typeof(Action<,,>));
            ActionTypes.Add(4, typeof(Action<,,,>));
            ActionTypes.Add(5, typeof(Action<,,,,>));
            ActionTypes.Add(6, typeof(Action<,,,,,>));
            ActionTypes.Add(7, typeof(Action<,,,,,,>));
            ActionTypes.Add(8, typeof(Action<,,,,,,,>));
            ActionTypes.Add(9, typeof(Action<,,,,,,,,>));
            ActionTypes.Add(10, typeof(Action<,,,,,,,,,>));
            ActionTypes.Add(11, typeof(Action<,,,,,,,,,,>));
            ActionTypes.Add(12, typeof(Action<,,,,,,,,,,,>));
            ActionTypes.Add(13, typeof(Action<,,,,,,,,,,,,>));
            ActionTypes.Add(14, typeof(Action<,,,,,,,,,,,,,>));
            ActionTypes.Add(15, typeof(Action<,,,,,,,,,,,,,,>));
            ActionTypes.Add(16, typeof(Action<,,,,,,,,,,,,,,,>));

            FuncTypes.Add(0, typeof(Func<>));
            FuncTypes.Add(1, typeof(Func<,>));
            FuncTypes.Add(2, typeof(Func<,,>));
            FuncTypes.Add(3, typeof(Func<,,,>));
            FuncTypes.Add(4, typeof(Func<,,,,>));
            FuncTypes.Add(5, typeof(Func<,,,,,>));
            FuncTypes.Add(6, typeof(Func<,,,,,,>));
            FuncTypes.Add(7, typeof(Func<,,,,,,,>));
            FuncTypes.Add(8, typeof(Func<,,,,,,,,>));
            FuncTypes.Add(9, typeof(Func<,,,,,,,,,>));
            FuncTypes.Add(10, typeof(Func<,,,,,,,,,,>));
            FuncTypes.Add(11, typeof(Func<,,,,,,,,,,,>));
            FuncTypes.Add(12, typeof(Func<,,,,,,,,,,,,>));
            FuncTypes.Add(13, typeof(Func<,,,,,,,,,,,,,>));
            FuncTypes.Add(14, typeof(Func<,,,,,,,,,,,,,,>));
            FuncTypes.Add(15, typeof(Func<,,,,,,,,,,,,,,,>));
            FuncTypes.Add(16, typeof(Func<,,,,,,,,,,,,,,,,>));
        }
        #endregion

        #region 缓存
        private static readonly Dictionary<string, Dictionary<string, Type>> _DelegateCache = new();
        #endregion

        /// <summary>
        /// 创建一个委托
        /// </summary>
        /// <param name="methodInfo">方法</param>
        /// <param name="instance">新的实例</param>
        /// <returns>委托</returns>
        public static Delegate CreateDelegate(MethodInfo methodInfo, object instance)
        {
            methodInfo.ThrowIfNull();

            Type delegateType = null!;
            List<Type> T = null!;
            if (_DelegateCache.TryGetValue(methodInfo.Name, out Dictionary<string, Type> MethodInfos))
                if (MethodInfos.Count == 1)
                {
                    delegateType = MethodInfos.FirstOrDefault().Value;
                    return CreateDelegate(delegateType, methodInfo, instance);
                }
                else if (MethodInfos.Count > 1)
                {
                    StringBuilder stringBuilder = new();
                    T = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
                    foreach (Type item in T)
                        _ = stringBuilder.Append(item.FullName);

                    if (MethodInfos.TryGetValue(stringBuilder.ToString(), out delegateType))
                        return CreateDelegate(delegateType, methodInfo, instance);
                }

            Type returnType = methodInfo.ReturnType;
            if (T.IsEmpty())
                T = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
            if (returnType.FullName == typeof(void).FullName)
            {
                delegateType = ActionTypes[T.Count];
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }
            else
            {
                delegateType = FuncTypes[T.Count];
                T.Add(returnType);
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }

            CacheDelegate(methodInfo.Name, string.Join(string.Empty, T.Select(x => x.Name)), delegateType);

            return CreateDelegate(delegateType, methodInfo, instance);
        }

        /// <summary>
        /// 创建委托
        /// </summary>
        /// <param name="delegateType">委托类型</param>
        /// <param name="methodInfo">方法</param>
        /// <param name="instance">实例</param>
        /// <returns>委托</returns>
        private static Delegate CreateDelegate(Type delegateType, MethodInfo methodInfo, object instance)
        {
            return instance == null
                ? Delegate.CreateDelegate(delegateType, methodInfo)
                : Delegate.CreateDelegate(delegateType, instance, methodInfo);
        }

        /// <summary>
        /// 缓存委托
        /// </summary>
        /// <param name="MethodName">方法名臣</param>
        /// <param name="ParamNames">参数名称</param>
        /// <param name="Delegate">委托类型</param>
        private static void CacheDelegate(string MethodName, string ParamNames, Type Delegate)
        {
            if (_DelegateCache.ContainsKey(MethodName))
                if (_DelegateCache[MethodName].ContainsKey(ParamNames))
                    _DelegateCache[MethodName][ParamNames] = Delegate;
                else
                    _DelegateCache[MethodName].Add(ParamNames, Delegate);
            else
                _DelegateCache.Add(MethodName, new Dictionary<string, Type> { { ParamNames, Delegate } });
        }
    }
}
