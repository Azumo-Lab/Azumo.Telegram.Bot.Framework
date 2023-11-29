using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    /// <summary>
    /// 图片数据库的模型
    /// </summary>
    internal class ImageInfo : DBBase
    {
        /// <summary>
        /// 图片保存的路径
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// 图片的文件名称
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// 显示的消息标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 图片的更加详细的信息
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 图片的标签
        /// </summary>
        public List<string> HashTag { get; set; } = [];

        /// <summary>
        /// 图片的Telegram ID
        /// </summary>
        public string? ImageID { get; set; }
    }
}
