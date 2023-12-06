using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    internal class ChannelInfoEntity : DBBase
    {
        /// <summary>
        /// 频道Chat ID
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// 频道的信息
        /// </summary>
        public string? ChannelInfo { get; set; }

        /// <summary>
        /// 频道名称
        /// </summary>
        public string? ChannelName { get; set; }

        /// <summary>
        /// 频道的用户名
        /// </summary>
        public string? ChannelUsername { get; set; }
    }
}
