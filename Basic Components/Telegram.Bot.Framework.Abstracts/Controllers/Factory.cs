//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal static class Factory
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly static IControllerParamMaker _controllerParamMaker = new ControllerParamMaker();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static IControllerParam GetControllerParam(ParameterInfo parameterInfo) =>
            _controllerParamMaker.Make(parameterInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ObjectFactory BuildObjectFactory(Type type) =>
            ActivatorUtilities.CreateFactory(type, []);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Func<object, object[], Task> BuildFunc(MethodInfo methodInfo)
        {
            // 获取方法的参数
            var methodParamInfos = methodInfo.GetParameters();

            // 设置参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var parameterExpression = Expression.Parameter(typeof(object[]), "args");
            var argsExpression = new Expression[methodParamInfos.Length];

            // 设置数组参数
            for (var i = 0; i < argsExpression.Length; i++)
            {
                var indexExpression = Expression.Constant(i);
                var arrayParam = Expression.ArrayIndex(parameterExpression, indexExpression);
                argsExpression[i] = Expression.Convert(arrayParam, methodParamInfos[i].ParameterType);
            }

            // 调用方法
            var invoker = Expression.Call(Expression.Convert(instance, methodInfo.DeclaringType!), methodInfo, argsExpression);
            Expression<Func<object, Task>> result = (obj) => obj as Task ?? Task.CompletedTask;
            var task = methodInfo.ReturnType != typeof(void)
                ? Expression.Invoke(result, invoker)
                : (Expression)Expression.Block(invoker, Expression.Invoke(result, Expression.Constant(new object(), typeof(object))));
            var express = Expression.Lambda<Func<object, object[], Task>>(task, instance, parameterExpression);
            return express.Compile();
        }

        public static Func<TelegramUserChatContext, IControllerParamManager, Task> BuildInvoker(ObjectFactory _objectFactory, Func<object, object[], Task> _func, Type controllerType)
        {
            var objArray = Expression.Constant(Array.Empty<object>());

            var factory = Expression.Constant(_objectFactory, typeof(ObjectFactory));
            var func = Expression.Constant(_func, typeof(Func<object, object[], Task>));
            var context = Expression.Parameter(typeof(TelegramUserChatContext));
            var iparam = Expression.Parameter(typeof(IControllerParamManager));

            Expression<Func<TelegramUserChatContext, IServiceProvider>> serviceProvider = (TelegramUserChatContext context) => context.UserScopeService;

            if (typeof(TelegramController).IsAssignableFrom(controllerType))
            {
                var obj = Expression.Invoke(factory, Expression.Invoke(serviceProvider, context), objArray);
                var call = Expression.Call(
                    Expression.Convert(obj, typeof(TelegramController)), 
                    typeof(TelegramController).GetMethod(nameof(TelegramController.ControllerInvokeAsync), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!,
                    context, func, iparam);
                return Expression
                    .Lambda<Func<TelegramUserChatContext, IControllerParamManager, Task>>(
                    call, context, iparam
                    ).Compile();
            }
            else
            {
                var obj = Expression.Invoke(factory, Expression.Invoke(serviceProvider, context), objArray);
                var result = Expression.Call(iparam, typeof(IControllerParamManager).GetMethod(nameof(IControllerParamManager.GetParams), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!);
                return Expression
                    .Lambda<Func<TelegramUserChatContext, IControllerParamManager, Task>>(
                    Expression.Invoke(func, obj, result), context, iparam
                    ).Compile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class ControllerParamMaker : IControllerParamMaker
        {
            /// <summary>
            /// 
            /// </summary>
            private static readonly List<(Type, TypeForAttribute)> _IControllerParamType;

            /// <summary>
            /// 
            /// </summary>
            /// <exception cref="Exception"></exception>
            static ControllerParamMaker()
            {
                var list = AzumoReflection<IControllerParam>.Reflection().GetAllSubClass();
                Type? type;
                if ((type = list
                    .Where(x => !Attribute.IsDefined(x, typeof(TypeForAttribute)))
                    .FirstOrDefault()) != null)
                    throw new Exception($"{type.FullName} 没有添加 {nameof(TypeForAttribute)} 属性，请添加对应的属性");

                _IControllerParamType = list.Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!)).ToList();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="parameterInfo"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public IControllerParam Make(ParameterInfo parameterInfo)
            {
                var controllerParam = _IControllerParamType
                    .Where(x => x.Item2.Type.FullName == parameterInfo.ParameterType.FullName)
                    .FirstOrDefault();

                var type = controllerParam.Item1;
                var newInvoker = type.GetConstructors().First();
                if (newInvoker.GetParameters().Length != 0)
                    throw new Exception($"暂时不支持具有参数的类型 {type.FullName}");

                IControllerParamSender controllerParamSender;
                var result = (newInvoker.Invoke([]) as IControllerParam)!
                    ?? throw new Exception($"{type.FullName} 似乎未实现 {nameof(IControllerParam)} 接口");

                controllerParamSender = new NULLControllerParamSender();
                if (Attribute.GetCustomAttribute(parameterInfo, typeof(ParamAttribute)) is ParamAttribute paramAttribute)
                {
                    if (paramAttribute.Sender != null)
                        controllerParamSender = (Activator.CreateInstance(paramAttribute.Sender) as IControllerParamSender)!;
                    result.ParamAttribute = paramAttribute;
                }
                result.ParamSender = controllerParamSender;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class NULLControllerParamSender : IControllerParamSender
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="botClient"></param>
            /// <param name="chatId"></param>
            /// <param name="paramAttribute"></param>
            /// <returns></returns>
            public Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute? paramAttribute) =>
                _ = botClient.SendTextMessageAsync(chatId, $"请输入参数{paramAttribute?.Name ?? string.Empty}的值");
        }
    }
}
