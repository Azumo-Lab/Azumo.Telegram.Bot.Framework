namespace MyChannel.DataBaseContext.DBModels
{
    /// <summary>
    /// 
    /// </summary>
    internal class UserInfo : DBBase
    {
        /// <summary>
        /// 用户ChatID
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public AuthEnum AuthEnum { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string? PassworHash { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 是否屏蔽这个用户
        /// </summary>
        public bool Blocked { get; set; }
    }
}
