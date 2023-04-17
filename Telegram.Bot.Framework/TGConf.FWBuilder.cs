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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Params;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 整个框架的数据收集，方法的编译等，进行框架上的一些事情的处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="TGConf_FWBuilder"/> 这个类是单例 <see cref="ServiceLifetime.Singleton"/> 的，并且拥有 <see cref="DependencyInjectionAttribute(ServiceLifetime)"/> 标签
    /// </para>
    /// <para>
    /// 这个类在调用<see cref="IServiceProvider.GetService(Type)"/>是实例化，并开始执行框架的构建操作。
    /// </para>
    /// </remarks>
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class TGConf_FWBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly List<Type> AllControllerType;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 当初始化的时候，开始进行框架的信息解析操作。这个操作发生在整个系统完全启动之前
        /// </remarks>
        /// <param name="serviceProvider">实例化传入的IServiceProvider</param>
        public TGConf_FWBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            // 获取所有的控制器类
            AllControllerType = typeof(TelegramController).GetSameType();

            // 解析所有的控制器
            foreach (Type ControllerType in AllControllerType)
            {
                ParseController(ControllerType);
            }

            List<Type> AllMakerType = typeof(IParamMaker).GetSameType();
            foreach (Type item in AllMakerType)
            {
                TypeForAttribute typeForAttribute = (TypeForAttribute)Attribute.GetCustomAttribute(item, typeof(TypeForAttribute));
                if (!typeForAttribute.IsNull())
                    ParamManager.__ParamType_MakerType.TryAdd(typeForAttribute.Type, item);
            }
        }

        /// <summary>
        /// 解析一个Controller
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void ParseController(Type type)
        {
            if (type.IsNull())
                throw new ArgumentNullException(nameof(type));

            IControllerContextFactory controllerContextFactory = serviceProvider.GetService<IControllerContextFactory>();

            // 获取方法信息
            MethodInfo[] allMethodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x =>
                    Attribute.IsDefined(x, typeof(BotCommandAttribute))
                    || Attribute.IsDefined(x, typeof(DefaultMessageAttribute))
                    || Attribute.IsDefined(x, typeof(DefaultTypeAttribute))
                ).ToArray();
            foreach (MethodInfo methodInfo in allMethodInfo)
            {
                IControllerContextBuilder controllerContextBuilder = serviceProvider.GetService<IControllerContextBuilder>();

                Func<TelegramController, object[], Task> Action = CompileDelegate(methodInfo);
                Attribute[] methodAttributes = Attribute.GetCustomAttributes(methodInfo);
                Attribute[] controllerAttributes = Attribute.GetCustomAttributes(methodInfo.DeclaringType);
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                controllerContextBuilder
                    .AddDelegate(Action)
                    .AddAttributes(methodAttributes)
                    .AddAttributes(controllerAttributes)
                    .AddParams(parameterInfos)
                    .AddMethodInfo(methodInfo);

                controllerContextFactory.AddContext(controllerContextBuilder);
            }
        }

        /// <summary>
        /// 将指定的方法转换成委托，增加执行效率
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        private Func<TelegramController, object[], Task> CompileDelegate(MethodInfo methodInfo)
        {
            // 创建表达式树
            ParameterExpression controller = Expression.Parameter(typeof(TelegramController), nameof(TelegramController).ToLower());
            ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

            ParameterInfo[] paramArray = methodInfo.GetParameters();
            List<Expression> expressionList = new List<Expression>();
            for (int i = 0; i < paramArray.Length; i++)
            {
                ParameterInfo param = paramArray[i];
                // 这一步是进行值转换，object[] 拆分成 -> object[] { string, int, ClassTest }
                expressionList.Add(Expression.Convert(Expression.ArrayIndex(argsParam, Expression.Constant(i)), param.ParameterType));
            }

            UnaryExpression ConvertController = Expression.Convert(controller, methodInfo.DeclaringType);
            MethodCallExpression call = Expression.Call(ConvertController, methodInfo, expressionList.ToArray());
            Expression<Func<TelegramController, object[], Task>> lambda = Expression.Lambda<Func<TelegramController, object[], Task>>(call, controller, argsParam);

            // 编译表达式树
            Func<TelegramController, object[], Task> Action = lambda.Compile();

            return Action;
        }
    }
}
