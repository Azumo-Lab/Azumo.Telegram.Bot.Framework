using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.UserAuthentication
{
    public interface IGlobalBlackList
    {
        public event EventHandler<UserIDArgs>? OnLoadUserID;

        public void Add(long userID);

        public void Remove(long userID);

        public bool Verify(long userID);
    }

    public class UserIDArgs : EventArgs
    {
        public List<long> UserIDs { get; } = [];
    }
}
