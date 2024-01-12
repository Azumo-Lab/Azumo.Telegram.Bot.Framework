using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal static class Factory
    {
        private readonly static IControllerParamMaker _controllerParamMaker = new ControllerParamMaker();
        public static IControllerParam GetControllerParam(ParameterInfo parameterInfo) =>
            _controllerParamMaker.Make(parameterInfo);

        public static ObjectFactory BuildObjectFactory(Type type) => 
            ActivatorUtilities.CreateFactory(type, []);

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

        private class ControllerParamMaker : IControllerParamMaker
        {
            private static readonly List<(Type, TypeForAttribute)> _IControllerParamType;

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

        private class NULLControllerParamSender : IControllerParamSender
        {
            public Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute paramAttribute) =>
                _ = botClient.SendTextMessageAsync(chatId, $"请输入参数{paramAttribute?.Name ?? string.Empty}的值");
        }
    }

    
}
