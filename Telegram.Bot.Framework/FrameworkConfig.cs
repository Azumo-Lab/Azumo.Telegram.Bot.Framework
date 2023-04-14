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
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Logger;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.MiddlewarePipelines;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.InternalImplementation.Params;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Framework.MiddlewarePipelines.Pipeline;
using Telegram.Bot.Framework.Attributes;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Framework.Abstract.Controller;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 框架配置
    /// 
    /// Expression表达式 微软文档
    /// https://learn.microsoft.com/zh-cn/dotnet/csharp/advanced-topics/expression-trees/expression-trees-interpreting
    /// Expression表达式的一个博客文章
    /// https://www.cnblogs.com/li-peng/p/3154381.html
    /// 
    /// 一个2D物理方面的项目，可以制作游戏
    /// https://github.com/Genbox/VelcroPhysics
    /// SkiaSharp 之 WPF 自绘 五环弹动球（案例版）
    /// https://www.cnblogs.com/kesshei/p/16538440.html
    /// SkiaSharp 之 WPF 自绘 粒子花园（案例版）
    /// https://www.cnblogs.com/kesshei/p/16548913.html
    /// SkiaSharp 之 WPF 自绘 投篮小游戏（案例版）
    /// https://www.cnblogs.com/kesshei/p/16552463.html
    /// Skia的文档
    /// https://skia.org/docs/user/api/skcanvas_overview/
    /// SkiaSharp的微软文档
    /// https://learn.microsoft.com/zh-cn/dotnet/api/skiasharp.skcanvas?view=skiasharp-2.88
    /// SkiaSharp项目i，可以用来制作游戏
    /// https://github.com/mono/SkiaSharp
    /// A-Star（A*）寻路算法原理与实现
    /// https://zhuanlan.zhihu.com/p/385733813
    /// 大佬的WPF游戏制作博文系列
    /// https://www.cnblogs.com/alamiye010/archive/2009/06/17/1505332.html
    /// https://www.cnblogs.com/alamiye010/archive/2009/06/17/1505346.html
    /// 
    /// 一个博客，主要内容是WPF，C#这一类的内容
    /// https://blog.lindexi.com/
    /// 
    /// C#中使用Socket实现简单Web服务器
    /// https://www.cnblogs.com/xiaozhi_5638/p/3917943.html
    /// 
    /// 火狐的文档，关于HTTP的一些
    /// https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Compression
    /// 
    /// 用 C# 自己动手编写一个 Web 服务器，第二部分——中间件
    /// https://shuhari.dev/blog/2017/12/build-web-server-middleware/
    /// 
    /// 支付宝当面付
    /// https://www.cnblogs.com/stulzq/p/7647948.html
    /// 
    /// C# ConcurrentBag的实现原理的一个博客文章
    /// https://www.cnblogs.com/incerry/p/9497729.html#2-%E7%94%A8%E4%BA%8E%E6%95%B0%E6%8D%AE%E5%AD%98%E5%82%A8%E7%9A%84threadlocallist%E7%B1%BB
    /// 
    /// 写一个简单数据库的博客文章
    /// https://www.cnblogs.com/kesshei/p/16519862.html
    /// </summary>
    internal class FrameworkConfig : IConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加使用标签的自动服务注册
            services.AddDependencyInjection();

            #region 中间件流水线相关的处理
            // 添加中间件流水线
            services.AddMiddlewarePipeline();
            services.AddMiddlewareTemplate();
            services.AddTransient<IPipelineBuilder, PipelineBuilder>();
            services.AddTransient<IPipelineController, PipelineController>();
            #endregion
        }
    }

    /// <summary>
    /// 框架配置的一些扩展方法
    /// </summary>
    internal static class FrameworkConfig_ExtensionMethod
    {
        public static void AddMiddlewarePipeline(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewarePipeline>(serviceDescriptors);
        }

        public static void AddMiddlewareTemplate(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewareTemplate>(serviceDescriptors);
        }

        public static void AddDependencyInjection(this IServiceCollection serviceDescriptors)
        {
            Type diAttr = typeof(DependencyInjectionAttribute);
            ObjectHelper.GetAllTypes()
                .Where(x => Attribute.IsDefined(diAttr, x))
                .ToList()
                .ForEach(x =>
                {
                    if (Attribute.GetCustomAttribute(x, diAttr) is not DependencyInjectionAttribute dependencyInjectionAttribute)
                        return;
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            serviceDescriptors.AddSingleton(dependencyInjectionAttribute.InterfaceType ?? x.BaseType ?? x, x);
                            break;
                        case ServiceLifetime.Scoped:
                            serviceDescriptors.AddScoped(dependencyInjectionAttribute.InterfaceType ?? x.BaseType ?? x, x);
                            break;
                        case ServiceLifetime.Transient:
                            serviceDescriptors.AddTransient(dependencyInjectionAttribute.InterfaceType ?? x.BaseType ?? x, x);
                            break;
                        default:
                            break;
                    }
                });
        }

        #region 将IServiceCollection 中的添加服务的方法扩展了

        private static void AddSingleton<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddSingleton(baseType, implType);
            });
        }

        private static void AddScoped<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddScoped(baseType, implType);
            });
        }

        private static void AddTransient<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddTransient(baseType, implType);
            });
        }

        private static void AddTemplate<T>(IServiceCollection serviceDescriptors, Action<IServiceCollection, Type, Type> action)
        {
            Type baseType = typeof(T);
            foreach (Type item in ObjectHelper.GetSameType(baseType))
            {
                action.Invoke(serviceDescriptors, baseType, item);
            }
        }

        #endregion
    }

    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class FrameworkBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly List<Type> AllControllerType;
        public FrameworkBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            // 获取所有的控制器类
            AllControllerType = typeof(TelegramController).GetSameType();

            // 解析所有的控制器
            foreach (Type ControllerType in AllControllerType)
            {
                ParseController(ControllerType);
            }
        }

        private void ParseController(Type type)
        {
            if (type.IsNull())
                throw new ArgumentNullException(nameof(type));

            IControllerContextFactory controllerContextFactory = serviceProvider.GetService<IControllerContextFactory>();

            // 获取方法信息
            MethodInfo[] allMethodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x =>
                    Attribute.IsDefined(x, typeof(BotCommandAttribute))
                ).ToArray();
            foreach (MethodInfo methodInfo in allMethodInfo)
            {
                IControllerContextBuilder controllerContextBuilder = serviceProvider.GetService<IControllerContextBuilder>();
                
                Action<TelegramController, object[]> Action = CompileDelegate(methodInfo);
                Attribute[] methodAttributes = Attribute.GetCustomAttributes(methodInfo);
                Attribute[] controllerAttributes = Attribute.GetCustomAttributes(methodInfo.DeclaringType);
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                controllerContextBuilder
                    .AddDelegate(Action)
                    .AddAttributes(methodAttributes)
                    .AddAttributes(controllerAttributes)
                    .AddParams(parameterInfos);

                controllerContextFactory.AddContext(controllerContextBuilder);
            }
        }

        private Action<TelegramController, object[]> CompileDelegate(MethodInfo methodInfo)
        {
            // 创建表达式树
            ParameterExpression containerParam = Expression.Parameter(typeof(TelegramController), nameof(TelegramController).ToLower());
            ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

            ParameterInfo[] paramArray = methodInfo.GetParameters();
            List<Expression> expressionList = new List<Expression>();
            for (int i = 0; i < paramArray.Length; i++)
            {
                ParameterInfo param = paramArray[i];
                // 这一步是进行值转换，object[] 拆分成 -> object[] { string, int, ClassTest }
                expressionList.Add(Expression.Convert(Expression.ArrayIndex(argsParam, Expression.Constant(i)), param.ParameterType));
            }

            MethodCallExpression call = Expression.Call(containerParam, methodInfo,
                expressionList.ToArray());
            Expression<Action<TelegramController, object[]>> lambda = Expression.Lambda<Action<TelegramController, object[]>>(call, containerParam, argsParam);

            // 编译表达式树
            Action<TelegramController, object[]> Action = lambda.Compile();
            
            return Action;
        }
    }
}
