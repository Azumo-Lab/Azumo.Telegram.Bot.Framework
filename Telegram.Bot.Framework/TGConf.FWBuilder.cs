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
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Params;

namespace Telegram.Bot.Framework
{
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class TGConf_FWBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly List<Type> AllControllerType;
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
