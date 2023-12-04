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

using System.Reflection;
using System.Runtime.CompilerServices;

namespace Azumo.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class AzReflection<T> : IDisposable
    {
        public static T? Create(params object[] args)
        {
            if (args == null || args.Length == 0)
                return (T?)Activator.CreateInstance(typeof(T));
            else
                return (T?)Activator.CreateInstance(typeof(T), args);
        }
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

        private static CacheType Cache<CacheType>(string cacheKey, Func<CacheType> cacheObj)
        {
            CacheType? result = default;
            if (!__CacheObjDic.TryGetValue(cacheKey, out object? obj))
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}