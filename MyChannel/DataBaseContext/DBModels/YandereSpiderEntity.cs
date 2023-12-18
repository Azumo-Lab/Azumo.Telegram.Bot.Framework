using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChannel.DataBaseContext.DBModels
{
    internal class YanderePoolEntity : DBBase
    {
        /// <summary>
        /// 图库名称
        /// </summary>
        public string? PoolName { get; set; }

        /// <summary>
        /// 图库中的图像
        /// </summary>
        public List<YandereSpiderEntity>? PoolItems { get; set; }
    }

    internal class YandereSpiderEntity : DBBase
    {
        /// <summary>
        /// 数据JSON内容
        /// </summary>
        public string? Json { get; set; }

        /// <summary>
        /// Json的文件路径
        /// </summary>
        public string? JsonPath { get; set; }

        /// <summary>
        /// 文件夹的路径
        /// </summary>
        public string? DirPath { get; set; }

        /// <summary>
        /// 图片的ID
        /// </summary>
        public string? ImageID { get; set; }

        /// <summary>
        /// 图片是否具有子图
        /// </summary>
        public bool HasChild { get; set; }

        /// <summary>
        /// 图片是否具有父类图
        /// </summary>
        public bool HasParent { get; set; }

        /// <summary>
        /// 有图库
        /// </summary>
        public bool HasPool { get; set; }

        /// <summary>
        /// 图片子图
        /// </summary>
        public List<YandereSpiderEntity>? Childs { get; set; }

        /// <summary>
        /// 图片父图
        /// </summary>
        public YandereSpiderEntity? Parent { get; set; }

        /// <summary>
        /// 图片的 TAG
        /// </summary>
        public List<YandereTagsEntity>? Tags { get; set; }

        /// <summary>
        /// 图库
        /// </summary>
        public List<YanderePoolEntity>? YanderePool { get; set; }

        /// <summary>
        /// 图片大小信息
        /// </summary>
        public YandereImageSizeEntity? ImageSize { get; set; }

        /// <summary>
        /// HTML文件的路径(方便以后查询数据)
        /// </summary>
        public string? HTMLPath { get; set; }

        /// <summary>
        /// 图像详情页面的链接
        /// </summary>
        public string? HTMLURL { get; set; }

        /// <summary>
        /// 完整图像的本地路径
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// 预览图像的本地路径
        /// </summary>
        public string? PreviewImagePath { get; set; }

        /// <summary>
        /// 完整图片的下载链接
        /// </summary>
        public string? ImageURL { get; set; }

        /// <summary>
        /// 预览图像下载链接
        /// </summary>
        public string? PreviewImageURL { get;set; }

        /// <summary>
        /// 图像来源连接
        /// </summary>
        public string? SouceURL { get; set; }

        /// <summary>
        /// 图像的年龄等级
        /// </summary>
        public YandereImageRankEntity? ImageRank { get; set; }
    }

    internal class YandereTagsEntity : DBBase
    {
        /// <summary>
        /// Tag 的名称
        /// </summary>
        public string? TagName { get; set; }

        /// <summary>
        /// Tag 的类型
        /// </summary>
        public string? TagTypeStr { get; set; }

        /// <summary>
        /// Tag 的类型
        /// </summary>
        public YandereImageTagType YandereImageTagType { get; set; }
    }

    internal class YandereImageSizeEntity : DBBase
    {
        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; set; }
    }

    public enum YandereImageRankEntity
    {
        /// <summary>
        /// 无等级
        /// </summary>
        None = 0,
        /// <summary>
        /// 健康全年龄
        /// </summary>
        Green = 1,
        /// <summary>
        /// 15+
        /// </summary>
        Yellow = 2,
        /// <summary>
        /// 18+
        /// </summary>
        Red = 3,
    }

    public enum YandereImageTagType
    {
        NONE = 10000,
        /// <summary>
        /// 艺术家
        /// </summary>
        Artist = 1,
        /// <summary>
        /// 作品
        /// </summary>
        Copyright = 3,
        /// <summary>
        /// 角色标签
        /// </summary>
        Character = 4,
        /// <summary>
        /// 普通标签
        /// </summary>
        General = 0,
        /// <summary>
        /// 团体社团
        /// </summary>
        Circle = 5,
        /// <summary>
        /// 
        /// </summary>
        Faults = 6,
    }
}
