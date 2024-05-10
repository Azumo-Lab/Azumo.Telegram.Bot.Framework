using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Args
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public HashSet<string>? Roles { get; internal set; }
    }
}
