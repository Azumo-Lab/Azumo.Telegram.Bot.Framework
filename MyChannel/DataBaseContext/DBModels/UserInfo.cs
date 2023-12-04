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
