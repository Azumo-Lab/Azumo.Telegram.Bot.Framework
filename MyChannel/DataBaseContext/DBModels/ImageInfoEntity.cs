using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    internal class ImageInfoEntity : DBBase
    {
        /// <summary>
        /// 图片的存放路径
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Telegram的文件ID
        /// </summary>
        public string? FileID { get; set; }

        /// <summary>
        /// 图片来源
        /// </summary>
        public string? SouceURL { get; set; }

        /// <summary>
        /// 描述内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Hash标签
        /// </summary>
        public List<HashTagInfoEntity>? HashTags { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// 作者主页
        /// </summary>
        public string? AuthorURL { get; set; }
    }
}
