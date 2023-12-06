namespace MyChannel.DataBaseContext.DBModels
{
    internal class MessageInfoEntity : DBBase
    {
        /// <summary>
        /// 发送后的消息ID
        /// </summary>
        public List<int>? MessageID { get; set; }

        /// <summary>
        /// 发送的消息标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 消息的TAG
        /// </summary>
        public List<HashTagInfoEntity>? Tags { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 消息附带的图片
        /// </summary>
        public List<ImageInfoEntity>? Images { get; set; }

        /// <summary>
        /// 消息附带的文件
        /// </summary>
        public List<FileIDInfoEntity>? FileIDs { get; set; }

        /// <summary>
        /// 这条消息是否等待发送中
        /// </summary>
        public bool WaitForSend { get; set; }

        /// <summary>
        /// 这条消息属于哪一个频道
        /// </summary>
        public ChannelInfoEntity? ChannelInfo { get; set; }
    }
}
