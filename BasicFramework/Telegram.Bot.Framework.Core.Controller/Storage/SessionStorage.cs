using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller.Storage;

/// <summary>
/// 
/// </summary>
[DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ISession))]
internal class SessionStorage : ISession
{
    /// <summary>
    /// 
    /// </summary>
    private readonly Dictionary<object, object> _cache = [];

    /// <summary>
    /// 
    /// </summary>
    public string ID { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 
    /// </summary>
    public int Count => _cache.Count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(object key, object value) => _cache.TryAdd(key, value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddOrUpdate(object key, object value)
    {
        if (!_cache.TryAdd(key, value))
            _cache[key] = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear() => _cache.Clear();

    /// <summary>
    /// 
    /// </summary>
    public void Dispose() => Clear();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object Get(object key)
    {
        _cache.TryGetValue(key, out var obj);
        return obj!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public void Remove(object key) => _cache.Remove(key);
}
