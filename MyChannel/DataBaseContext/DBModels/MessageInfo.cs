namespace MyChannel.DataBaseContext.DBModels
{
    /// <summary>
    /// 
    /// </summary>
    internal class MessageInfo : DBBase
    {
        /// <summary>
        /// 
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? HtmlContent { get; set; }

    }
}
