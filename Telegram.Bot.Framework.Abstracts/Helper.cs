using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts
{
    public static class Helper
    {
        public static List<Type> AllTypes { get; } = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

        private static readonly Dictionary<string, List<Type>> __TypeCache = new();

        public static List<Type> FindTypes(this object obj)
        {
            if (obj is not Type baseType)
                baseType = obj.GetType();

            if (baseType.IsNull())
                throw new NullReferenceException(nameof(baseType));

            if (!__TypeCache.TryGetValue(baseType.FullName!, out List<Type>? typeList))
            {
                typeList = AllTypes.Where(x =>
                    !x.IsInterface && !x.IsAbstract && baseType.IsAssignableFrom(x)
                ).ToList();

                __TypeCache.TryAdd(baseType.FullName!, typeList);
            }

            return typeList;
        }

        public static List<T> FindTypes<T>(this object obj)
        {
            return FindTypes(obj).Select(Activator.CreateInstance).Select(x => (T)x!).Where(x => x != null).ToList();
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 使用 <see cref="DependencyInjectionAttribute"/> 标签
        /// </summary>
        /// <remarks>
        /// 程序会寻找 <see cref="DependencyInjectionAttribute"/> 标签，并将指定的类和接口注册<br></br>
        /// 如果一个类型实现了多个接口，但是没有指定注册的类型，则会抛出 <see cref="NotSupportedException"/> 异常
        /// </remarks>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">未提供支持</exception>
        public static IServiceCollection UseDependencyInjectionAttribute(this IServiceCollection serviceDescriptors)
        {
            Type objType = typeof(object);
            Type diAttr = typeof(DependencyInjectionAttribute);
            AllTypes
                .Where(x => Attribute.IsDefined(x, diAttr))
                .ToList()
                .Select(x =>
                {
                    return (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, diAttr));
                })
                .OrderBy(x => x.Item2.Priority)
                .Select(x => x.x)
                .ToList()
                .ForEach(x =>
                {
                    if (Attribute.GetCustomAttribute(x, diAttr) is not DependencyInjectionAttribute dependencyInjectionAttribute)
                        return;

                    Type baseType;
                    Type[] interFaceType;
                    Type serviceType = dependencyInjectionAttribute.ServiceType ??
                            (((baseType = x.BaseType).FullName == objType.FullName)
                            ? ((interFaceType = x.GetInterfaces()).Length > 1
                                ? throw new NotSupportedException($"在 {x.FullName} 中，检测到多个接口类型：{string.Join(',', interFaceType.Select(x => x.FullName).ToList())}")
                                : interFaceType.Length == 0 ? x : interFaceType[0])
                            : baseType);
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = serviceDescriptors.AddSingleton(serviceType, x);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = serviceDescriptors.AddScoped(serviceType, x);
                            break;
                        case ServiceLifetime.Transient:
                            _ = serviceDescriptors.AddTransient(serviceType, x);
                            break;
                        default:
                            break;
                    }
                });
            return serviceDescriptors;
        }

        /// <summary>
        /// 获取参数， -Token "Token" -Proxy "http://127.0.0.1:7890/"
        /// </summary>
        /// <param name="args">参数列表</param>
        /// <param name="name">想要捕获的参数</param>
        /// <returns>返回的是想要的参数</returns>
        public static string GetArgs(this string[] args, string name)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower() == name.ToLower() || arg.ToUpper() == name.ToUpper())
                    return args.GetVal(i);
            }
            return string.Empty;
        }

        #region 字符串类型，处理，扩展方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return str.IsNull() || str.IsEmpty();
        }

        public static bool IsNullOrTrimEmpty(this string str)
        {
            return str.IsNull() || str.Trim().IsEmpty();
        }

        public static bool IsEmpty(this string str)
        {
            return str == string.Empty;
        }

        public static string GetVal(this string str, string defVal)
        {
            return str.IsNullOrEmpty() ? defVal : str;
        }

        #endregion

        #region Object类型的处理，扩展方法

        /// <summary>
        /// 判断对象是否是空
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <returns>布尔值，Null为True，反之</returns>
        public static bool IsNull([AllowNull]this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="objs">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull([AllowNull]params object[] objs)
        {
            if (objs.IsNull())
                return true;
            foreach (object item in objs!)
            {
                if (item.IsNull())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="arrays">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull<T>(this T[] arrays)
        {
            return HasAnyNull(arrays);
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="list">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull<T>(this List<T> list)
        {
            if (list.IsNull())
                return true;
            foreach (T item in list)
            {
                if (item.IsNull())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断对象中的对象是否都是Null
        /// </summary>
        /// <param name="objs">要判断的对象</param>
        /// <returns>布尔值，数组所有元素皆为Null为True，反之</returns>
        public static bool HasAllNull(params object[] objs)
        {
            if (objs.IsNull())
                return true;
            foreach (object item in objs)
            {
                if (!item.IsNull())
                    return false;
            }
            return true;
        }

        #endregion

        #region 文件类的扩展处理
        public static bool IsFilePath(this string path)
        {
            return File.Exists(path);
        }
        #endregion

        #region 数组类型的处理

        public static T GetVal<T>(this T[] list, int index)
        {
            int newIndex = 0;
            for (int i = 0; i < index; i++)
            {
                newIndex++;
                if (newIndex >= list.Length)
                    newIndex = 0;
            }
            return list[newIndex];
        }

        public static bool IsEmpty<T>(this T[] list)
        {
            return list.IsNull() || list.Length == 0;
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.IsNull() || list.Count == 0;
        }

        #endregion

        public static Chat? GetChat(this Update update)
        {
            switch (update.Type)
            {
                case Types.Enums.UpdateType.Unknown:
                    return null;
                case Types.Enums.UpdateType.Message:
                    return update.Message?.Chat;
                case Types.Enums.UpdateType.InlineQuery:
                    break;
                case Types.Enums.UpdateType.ChosenInlineResult:
                    break;
                case Types.Enums.UpdateType.CallbackQuery:
                    return update.CallbackQuery?.Message?.Chat;
                case Types.Enums.UpdateType.EditedMessage:
                    break;
                case Types.Enums.UpdateType.ChannelPost:
                    break;
                case Types.Enums.UpdateType.EditedChannelPost:
                    break;
                case Types.Enums.UpdateType.ShippingQuery:
                    break;
                case Types.Enums.UpdateType.PreCheckoutQuery:
                    break;
                case Types.Enums.UpdateType.Poll:
                    break;
                case Types.Enums.UpdateType.PollAnswer:
                    break;
                case Types.Enums.UpdateType.MyChatMember:
                    break;
                case Types.Enums.UpdateType.ChatMember:
                    break;
                case Types.Enums.UpdateType.ChatJoinRequest:
                    break;
                default:
                    break;
            }
        }

        public static Types.User? GetChatUser(this Update update)
        {
            switch (update.Type)
            {
                case Types.Enums.UpdateType.Unknown:
                    break;
                case Types.Enums.UpdateType.Message:
                    break;
                case Types.Enums.UpdateType.InlineQuery:
                    break;
                case Types.Enums.UpdateType.ChosenInlineResult:
                    break;
                case Types.Enums.UpdateType.CallbackQuery:
                    break;
                case Types.Enums.UpdateType.EditedMessage:
                    break;
                case Types.Enums.UpdateType.ChannelPost:
                    break;
                case Types.Enums.UpdateType.EditedChannelPost:
                    break;
                case Types.Enums.UpdateType.ShippingQuery:
                    break;
                case Types.Enums.UpdateType.PreCheckoutQuery:
                    break;
                case Types.Enums.UpdateType.Poll:
                    break;
                case Types.Enums.UpdateType.PollAnswer:
                    break;
                case Types.Enums.UpdateType.MyChatMember:
                    break;
                case Types.Enums.UpdateType.ChatMember:
                    break;
                case Types.Enums.UpdateType.ChatJoinRequest:
                    break;
                default:
                    break;
            }
        }

        public static Types.User GetSendUser(this Update update)
        {
            switch (update)
            {

            }
        }
    }
}
