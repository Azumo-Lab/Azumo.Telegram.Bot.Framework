using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Channel.Controllers
{
    internal class HashMap<K, V> : IDictionary<K, V>
    {
        private readonly Node<K, V>[] _node;

        public V this[K key] { get => _node[GetHashIndex(key, _node.Length)].Get(key); set => throw new NotImplementedException(); }

        public ICollection<K> Keys => throw new NotImplementedException();

        public ICollection<V> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => false;

        public HashMap()
        {
            _node = new Node<K, V>[1024];
        }

        public void Add(K key, V value)
        {
            int index = GetHashIndex(key, _node.Length);

            if (_node[index] == null)
            {
                _node[index] = new Node<K, V>();
            }
            _node[index].Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(K key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(K key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private static int GetHashIndex<KEY>(KEY key, int len)
        {
            if (key == null)
                return 0;

            int index = key.GetHashCode();
            return (index ^ index >> 16) & len - 1;
        }

        private class Node<Kn, Vn>
        {
            private Vn[] _Vns = new Vn[32];
            public Vn Get(Kn k)
            {
                int index = GetHashIndex(k, _Vns.Length);
                return _Vns[index];
            }

            public void Add(Kn k, Vn vn)
            {
                int index = GetHashIndex(k, _Vns.Length);
                _Vns[index] = vn;
            }
        }
    }
}
