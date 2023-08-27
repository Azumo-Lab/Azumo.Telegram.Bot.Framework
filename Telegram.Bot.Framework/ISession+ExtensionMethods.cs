using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework
{
    public static class ISession_ExtensionMethods
    {
        public static T Get<T>(this ISession session, string key)
        {
            object obj;
            if ((obj = session.Get(key)) != null)
                try
                {
                    return (T)obj;
                }
                catch (Exception)
                {
                    return default;
                }
            return default;
        }
    }
}
