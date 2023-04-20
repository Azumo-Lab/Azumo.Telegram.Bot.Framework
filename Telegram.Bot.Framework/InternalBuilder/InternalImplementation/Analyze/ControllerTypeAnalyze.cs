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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalBuilder.Abstract.Analyze;
using Telegram.Bot.Framework.InternalBuilder.InternalImplementation.Models;

namespace Telegram.Bot.Framework.InternalBuilder.InternalImplementation.Analyze
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllerTypeAnalyze : AbsAnalyze, ITypeAnalyze
    {

        public ControllerTypeAnalyze(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IMethodFilter, ControllerMethodFilter>();
        }

        public ControllerTypeAnalyze(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Analyze()
        {
            List<ControllerModels> ControllerModelsList = new();
            List<Type> ControllerTypes = TypeFinder.FindTypes<TelegramController>();
            ControllerTypes.ForEach(ControllerType =>
            {
                ControllerModels ControllerModel = new();
                ControllerModel.Attributes.AddRange(Attribute.GetCustomAttributes(ControllerType));

                IEnumerable<IMethodFilter> methodFilters = ServiceProvider.GetServices<IMethodFilter>();

                ControllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(methodInfo => !methodFilters.Where(x => !x.GetFunc().Invoke(methodInfo)).Any())
                .ToList().ForEach(Method =>
                {
                    MethodModels MethodModel = new() { MethodInfo = Method };
                    MethodModel.Attribute.AddRange(Attribute.GetCustomAttributes(Method));

                    Func<TelegramController, object[], Task> TaskFun = CompileDelegate(Method);
                    MethodModel.Action = TaskFun;

                    Method.GetParameters().ToList().ForEach(Param =>
                    {
                        MethodModel.ParamModels.Add(new ParamModels
                        {
                            Attributes = Attribute.GetCustomAttributes(Param).ToList(),
                            ParameterInfo = Param
                        });
                    });

                    ControllerModel.MethodModels.Add(MethodModel);
                });
                ControllerModelsList.Add(ControllerModel);
            });
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
            List<Expression> expressionList = new();
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
