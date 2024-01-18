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

namespace Azumo.Reflection;

/// <summary>
/// 
/// </summary>
public class AzReflection<T> : IDisposable
{
    public static T? Create(params object[] args) => args == null || args.Length == 0 ? (T?)Activator.CreateInstance(typeof(T)) : (T?)Activator.CreateInstance(typeof(T), args);
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
    static AzReflection() => __AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    private AzReflection(Type type) => __Type = type;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static AzReflection<T> Create() => new(typeof(T));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Type> FindAllSubclass() => Cache(CacheKey(__Type.FullName!, nameof(FindAllSubclass)), () => __AllTypes.Where(__Type.IsAssignableFrom).Where(x => !x.IsInterface && !x.IsAbstract).ToList());

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Func<T, object[], object?>> GetFuncMethods()
    {
        var funcs = Cache(CacheKey(__Type.FullName!, nameof(GetFuncMethods)), () => GetMethods().Select<MethodInfo, Func<T, object[], object?>>(x =>
            {
                RuntimeHelpers.PrepareMethod(x.MethodHandle);
                return (t, param) => x.Invoke(t, param);
            }).ToList());
        foreach (var func in funcs)
            RuntimeHelpers.PrepareDelegate(func);
        return funcs;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<MethodInfo> GetMethods()
    {
        var methods = Cache(CacheKey(__Type.FullName!, nameof(GetMethods)), () => __Type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
        return [.. (methods ?? [])];
    }

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

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {

    }
}