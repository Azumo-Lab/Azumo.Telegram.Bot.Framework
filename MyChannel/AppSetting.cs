namespace MyChannel
{
    /// <summary>
    /// 
    /// </summary>
    internal class AppSetting
    {
        /// <summary>
        /// 
        /// </summary>
        public Images? ImagesInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class Images
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// 已经发送的图片路径
        /// </summary>
        public string? SendImagePath { get; set; }
    }
}
