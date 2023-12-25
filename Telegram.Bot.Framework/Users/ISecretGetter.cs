using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Users
{
    public interface ISecretGetter
    {
        public Task<string> GetSecret();
    }
}
