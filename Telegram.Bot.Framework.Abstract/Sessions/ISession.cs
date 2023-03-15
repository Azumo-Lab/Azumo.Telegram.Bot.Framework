using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    public interface ISession
    {
        public string SessionID { get; }

        public void Save(object sessionKey, byte[] data);

        public byte[] Get(object sessionKey);

        public void Delete(object sessionKey);
    }
}
