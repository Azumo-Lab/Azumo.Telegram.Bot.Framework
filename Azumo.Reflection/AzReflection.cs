using System.Reflection;
using System.Runtime.CompilerServices;

namespace Azumo.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class AzReflection<T> : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Type __Type;

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<Type> __AllTypes;

        /// <summary>
        /// 
        /// </summary>
        static AzReflection()
        {
            __AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private AzReflection(Type type)
        {
            __Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static AzReflection<T> Create()
        {
            return new AzReflection<T>(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Type> FindAllSubclass()
        {
            return Cache(CacheKey(__Type.FullName!, nameof(FindAllSubclass)), () => __AllTypes.Where(__Type.IsAssignableFrom).Where(x => !x.IsInterface && !x.IsAbstract).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Func<T, object[], object?>> GetFuncMethods()
        {
            List<Func<T, object[], object?>> funcs = Cache(CacheKey(__Type.FullName!, nameof(GetFuncMethods)), () =>
            {
                return GetMethods().Select<MethodInfo, Func<T, object[], object?>>(x =>
                {
                    RuntimeHelpers.PrepareMethod(x.MethodHandle);
                    return (t, param) =>
                    {
                        return x.Invoke(t, param);
                    };
                }).ToList();
            });
            foreach (Func<T, object[], object?> func in funcs)
                RuntimeHelpers.PrepareDelegate(func);
            return funcs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MethodInfo> GetMethods()
        {
            MethodInfo[] methods = Cache(CacheKey(__Type.FullName!, nameof(GetMethods)), () =>
            {
                return __Type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            });
            return (methods ?? Array.Empty<MethodInfo>()).ToList();
        }

        #region 保存缓存

        private static readonly Dictionary<string, object> __CacheObjDic = new();

        private static string CacheKey(string typeFullName, string methodName)
        {
            return $"{typeFullName}.{methodName}";
        }

        private static T Cache<T>(string cacheKey, Func<T> cacheObj)
        {
            T? result = default;
            if (!__CacheObjDic.TryGetValue(cacheKey, out object? obj))
            {
                result = cacheObj();
                _ = __CacheObjDic.TryAdd(cacheKey, result!);
            }
            else
            {
                try
                {
                    result = (T)obj;
                }
                catch (Exception)
                { }
            }
            return result!;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}