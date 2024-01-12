using Azumo.Reflection.MethodReflections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public class AzumoReflection<T> : AzumoTypeDescriptorBase
    {
        public AzumoReflection() : base(typeof(T))
        {
        }

        public Type Type { get; } = typeof(T);

        public static AzumoReflection<T> Reflection() => new();

        public AzumoMethodReflectionInfo[] GetMethods(BindingFlags bindingFlags) => 
            Type.GetMethods(bindingFlags)
                .Select(x => new AzumoMethodReflectionInfo(x))
                .ToArray();

        public List<Type> GetAllSubClass() =>
             Cache(CacheKey(Type.FullName!, nameof(GetAllSubClass)), 
                 () => AzumoReflection.AllTypes
                    .Where(Type.IsAssignableFrom)
                    .Where(x => !x.IsInterface && !x.IsAbstract).ToList());

        #region 保存缓存

        private static readonly Dictionary<string, object> __CacheObjDic = [];

        private static string CacheKey(string typeFullName, string methodName) => $"{typeFullName}.{methodName}";

        private static CacheType Cache<CacheType>(string cacheKey, Func<CacheType> cacheObj)
        {
            CacheType? result = default;
            if (!__CacheObjDic.TryGetValue(cacheKey, out var obj))
            {
                result = cacheObj();
                _ = __CacheObjDic.TryAdd(cacheKey, result!);
            }
            else
            {
                try
                {
                    result = (CacheType)obj;
                }
                catch (Exception)
                { }
            }
            return result!;
        }

        #endregion
    }

    public class AzumoReflection
    {
        public static List<Type> AllTypes { get; } = 
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
    }
}
