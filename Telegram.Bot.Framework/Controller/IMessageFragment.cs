using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageFragment
    {
        /// <summary>
        /// 
        /// </summary>
        public FragmentType FragmentType { get; }

        /// <summary>
        /// 
        /// </summary>
        public string DataInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        public Stream[]? Data { get; }
    }
}
