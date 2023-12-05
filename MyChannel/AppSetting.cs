namespace MyChannel
{
    /// <summary>
    /// 
    /// </summary>
    internal class AppSetting
    {
        /// <summary>
        /// Bot连接用的Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 图片相关的设置
        /// </summary>
        public ImageSetting? ImageSetting { get; set; }

        /// <summary>
        /// Log的相关设置
        /// </summary>
        public LogSetting? LogSetting { get; set; }

        /// <summary>
        /// 数据库的相关设置
        /// </summary>
        public DatabaseSetting? DatabaseSetting { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ImageSetting
    {
        /// <summary>
        /// Bot从网络下载的图片路径
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Bot从Telegram上面接受到的图片
        /// </summary>
        /// <remarks>
        /// 这一类图片会用JSON文件保存其中的图片ID信息
        /// </remarks>
        public string? TelegramImageIDPath { get; set; }
    }

    internal class LogSetting
    {
        /// <summary>
        /// Log保存的位置
        /// </summary>
        public string? LogPath { get; set; }
    }

    internal class DatabaseSetting
    {
        /// <summary>
        /// 连接用字符串
        /// </summary>
        public string? ConnectionString { get; set; }
    }
}
