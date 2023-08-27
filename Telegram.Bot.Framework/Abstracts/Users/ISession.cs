using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    public interface ISession
    {
        public void Set(string key, object value);

        public object Get(string key);

        public bool Remove(string key);

        public bool HasVal(string key);

        public bool TryGetValue(string key, out object value);
    }
}
