using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    /// <summary>
    /// 
    /// </summary>
    internal class UserInfo : DBBase
    {
        /// <summary>
        /// 
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthEnum AuthEnum { get; set; }
    }
}
