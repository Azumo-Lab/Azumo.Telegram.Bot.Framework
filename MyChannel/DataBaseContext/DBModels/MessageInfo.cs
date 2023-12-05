using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    internal class MessageInfo : DBBase
    {
        /// <summary>
        /// 发送后的消息ID
        /// </summary>
        public int? MessageID { get; set; }

        /// <summary>
        /// 发送的消息标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 消息的TAG
        /// </summary>
        public List<HashTagInfo>? Tags { get; set; }
    }
}
