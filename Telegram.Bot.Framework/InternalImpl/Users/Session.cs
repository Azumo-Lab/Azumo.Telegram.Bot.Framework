using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalImpl.Users
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(ISession))]
    internal class Session : ISession
    {
        private readonly Dictionary<string, object> __Val = new Dictionary<string, object>();
        public object Get(string key)
        {
            return TryGetValue(key, out object value) ? value : null;
        }

        public bool HasVal(string key)
        {
            return __Val.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return __Val.Remove(key);
        }

        public void Set(string key, object value)
        {
            __Val.TryAdd(key, value);
        }

        public bool TryGetValue(string key, out object value)
        {
            return __Val.TryGetValue(key, out value);
        }
    }
}
