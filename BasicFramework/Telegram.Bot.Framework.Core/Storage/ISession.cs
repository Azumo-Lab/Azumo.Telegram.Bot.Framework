using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Storage;

/// <summary>
/// 
/// </summary>
public interface ISession
{
    /// <summary>
    /// 
    /// </summary>
    public string ID { get; }

    /// <summary>
    /// 
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(object key, object value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public void Remove(object key);

    /// <summary>
    /// 
    /// </summary>
    public void Clear();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object Get(object key);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddOrUpdate(object key, object value);
}
