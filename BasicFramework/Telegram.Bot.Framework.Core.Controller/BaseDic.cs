namespace Telegram.Bot.Framework.Core.Controller;
internal class BaseDictionary<K, V> where K : notnull
{
    private readonly Dictionary<K, V> _dictionary = [];

    public bool TryAdd(K key, V value) => _dictionary.TryAdd(key, value);

    public void AddOrUpdate(K key, V value)
    {
        if (!TryAdd(key, value))
            _dictionary[key] = value;
    }

    public V? Get(K key)
    {
        TryGet(key, out var value);
        return value;
    }

    public bool TryGet(K key, out V? v) => 
        _dictionary.TryGetValue(key, out v);

    public bool TryGet(K key, out V v, Func<V> defVal)
    {
        bool result;
        if (!(result = TryGet(key, out var outVal)))
            outVal = defVal();
        v = outVal!;
        return result;
    }

    public bool ContainsKey(K key) => 
        _dictionary.ContainsKey(key);
}
